__author__ = 'TED'

import subprocess
import sys


def main():
	try:
		if not commit():
			return
		subprocess.check_call("git checkout master", shell=True)

		build_docs()

		commit_docs()

		subprocess.check_call("git push origin --all --prune")
	except subprocess.SubprocessError:
		print("The script failed to run. Execution aborted.")


def commit():
	status_out = subprocess.Popen("git status -s", shell=True, stdout=subprocess.PIPE).stdout.read()
	if status_out == "b''":
		print("Nothing to commit")
		return True

	print("You have uncommited changes.")
	#sys.stdout.write(status_out)
	#sys.stdout.flush()
	print(str(status_out), "UTF-8")
	print("Enter commit message or ABORT to abort.")
	message = input()
	if message == "ABORT":
		return False
	subprocess.check_call("git add -A", shell=True)
	subprocess.check_call("git commit \"" % message % "\"", shell=True)

	print("Changes commited.")


def build_docs():
	print("Cleaning past docs...")
	subprocess.call("DEL /S /Q __doxygen\html\*.*", shell=True)
	subprocess.call("DEL __doxygen\Docs.zip", shell=True)

	print("Building new docs...")

	subprocess.check_call("cd __doxygen", shell=True)

	subprocess.check_call("doxygen config", shell=True)
	subprocess.check_call("7z a -tzip Docs.zip html\\", shell=True)

	subprocess.call("cd ..", shell=True)

	print("Docs built.")


def commit_docs():
	print("Commiting docs...")
	subprocess.check_call("git checkout gh_pages")

	subprocess.check_call("git add -A", shell=True)
	subprocess.check_call("git commit \"Docs updated.\"", shell=True)

	print("Adding Download Zip link to the docs.")
	subprocess.check_call("git cherry-pick dd434a0")

if __name__ == "__main__":
	main()
