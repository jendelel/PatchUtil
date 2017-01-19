import patch
import os
import logging
import sys
from os.path import exists, isfile, abspath

#------------------------------------------------
# Logging is controlled by logger named after the
# module name (e.g. 'patch' for patch.py module)

logger = logging.getLogger(__name__)

debug = logger.debug
info = logger.info
warning = logger.warning

class NullHandler(logging.Handler):
  """ Copied from Python 2.7 to avoid getting
      `No handlers could be found for logger "patch"`
      http://bugs.python.org/issue16539
  """
  def handle(self, record):
    pass
  def emit(self, record):
    pass
  def createLock(self):
    self.lock = None

streamhandler = logging.StreamHandler()

# initialize logger itself
logger.addHandler(NullHandler())

def my_apply(patchset, orig_file, dest_file, strip=0, root=None):
    """ Apply parsed patch, optionally stripping leading components
        from file paths. `root` parameter specifies working dir.
        return True on success
    """
    if root:
      prevdir = os.getcwd()
      os.chdir(root)

    total = len(patchset.items)
    errors = 0
    if strip:
      # [ ] test strip level exceeds nesting level
      #   [ ] test the same only for selected files
      #     [ ] test if files end up being on the same level
      try:
        strip = int(strip)
      except ValueError:
        errors += 1
        warning("error: strip parameter '%s' must be an integer" % strip)
        strip = 0

    #for fileno, filename in enumerate(patchset.source):
    for i,p in enumerate(patchset.items):
      if strip:
        debug("stripping %s leading component(s) from:" % strip)
        debug("   %s" % p.source)
        debug("   %s" % p.target)
        old = pathstrip(p.source, strip)
        new = pathstrip(p.target, strip)
      else:
        old, new = p.source, p.target

      if old and old != orig_file:
        continue
      elif new and new != orig_file:
        continue
        
      filename = patchset.findfile(old, new)

      if not filename:
          warning("source/target file does not exist:\n  --- %s\n  +++ %s" % (old, new))
          errors += 1
          continue
      if not isfile(filename):
        warning("not a file - %s" % filename)
        errors += 1
        continue

      # [ ] check absolute paths security here
      debug("processing %d/%d:\t %s" % (i+1, total, filename))

      # validate before patching
      f2fp = open(filename, 'rb')
      hunkno = 0
      hunk = p.hunks[hunkno]
      hunkfind = []
      hunkreplace = []
      validhunks = 0
      canpatch = False
      for lineno, line in enumerate(f2fp):
        if lineno+1 < hunk.startsrc:
          continue
        elif lineno+1 == hunk.startsrc:
          hunkfind = [x[1:].rstrip(b"\r\n") for x in hunk.text if x[0] in b" -"]
          hunkreplace = [x[1:].rstrip(b"\r\n") for x in hunk.text if x[0] in b" +"]
          #pprint(hunkreplace)
          hunklineno = 0

          # todo \ No newline at end of file

        # check hunks in source file
        if lineno+1 < hunk.startsrc+len(hunkfind)-1:
          if line.rstrip(b"\r\n") == hunkfind[hunklineno]:
            hunklineno+=1
          else:
            info("file %d/%d:\t %s" % (i+1, total, filename))
            info(" hunk no.%d doesn't match source file at line %d" % (hunkno+1, lineno+1))
            info("  expected: %s" % hunkfind[hunklineno])
            info("  actual  : %s" % line.rstrip(b"\r\n"))
            # not counting this as error, because file may already be patched.
            # check if file is already patched is done after the number of
            # invalid hunks if found
            # TODO: check hunks against source/target file in one pass
            #   API - check(stream, srchunks, tgthunks)
            #           return tuple (srcerrs, tgterrs)

            # continue to check other hunks for completeness
            hunkno += 1
            if hunkno < len(p.hunks):
              hunk = p.hunks[hunkno]
              continue
            else:
              break

        # check if processed line is the last line
        if lineno+1 == hunk.startsrc+len(hunkfind)-1:
          debug(" hunk no.%d for file %s  -- is ready to be patched" % (hunkno+1, filename))
          hunkno+=1
          validhunks+=1
          if hunkno < len(p.hunks):
            hunk = p.hunks[hunkno]
          else:
            if validhunks == len(p.hunks):
              # patch file
              canpatch = True
              break
      else:
        if hunkno < len(p.hunks):
          warning("premature end of source file %s at hunk %d" % (filename, hunkno+1))
          errors += 1

      f2fp.close()

      if validhunks < len(p.hunks):
        if patchset._match_file_hunks(filename, p.hunks):
          print("already patched  {}".format(filename))
          from shutil import copyfile
          copyfile(filename, dest_file)
        else:
          print("source file is different - {}".format(filename))
          errors += 1
      if canpatch:
          if patchset.write_hunks(filename, dest_file, p.hunks):
            print("successfully patched {}/{}:\t {}".format(i+1, total, filename))
          else:
            errors += 1
            print("error patching file {}".format(filename))
            print("invalid version is saved to {}".format(dest_file))

    if root:
      os.chdir(prevdir)

    # todo: check for premature eof
    return (errors == 0)



if __name__ == "__main__":
    import argparse
    parser = argparse.ArgumentParser()
    parser.add_argument("--patch_file", default="", type=str, help="Patch file to parse.")
    parser.add_argument("--cmd", default="ls", type=str, help="Command to run.")
    parser.add_argument("--work_dir", default="", type=str, help="Original directory.")
    parser.add_argument("--orig_file", default="", type=str, help="Original file.")
    parser.add_argument("--target_file", default="", type=str, help="File to patch.")
    
    args = parser.parse_args()
    
    if len(args.work_dir) > 0:
        os.chdir(args.work_dir)
    else:
        os.chdir(os.path.dirname(args.patch_file))
    
    parsed = patch.fromfile(args.patch_file)
    if args.cmd == 'ls':
        for i, p in enumerate(parsed.items):
            old, new = p.source, p.target
            if old:
                print(old)
            else:
                print(new)
    elif args.cmd == 'apply':
        orig_file = args.orig_file
        dest_file = args.target_file
        my_apply(parsed, orig_file, dest_file) or sys.exit(-1)
    elif args.cmd == 'applyall':
        parsed.apply() or sys.exit(-2)    