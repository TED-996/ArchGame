__author__ = 'TED'

import subprocess
import sys


def main():
	stdout = subprocess.DEVNULL
	if sys.argv.__contains__("-v"):
		stdout = None

	try:

		if not commit(stdout):
			return
		subprocess.check_call("git checkout master", shell=True, stdout=stdout)

		build_docs(stdout)

		commit_docs(stdout)
		
		print("Pushing changes...")
		subprocess.check_call("git push origin --all --prune", stdout=stdout, stderr=stdout)

	except subprocess.SubprocessError as ex:
		print("The script failed to run. Execution aborted.")
		print(ex)

	print("Script done.")


def commit(stdout):
	print("Commiting changes.")
	#possible workaround the main.py error
	#status = subprocess.Popen("git status -s", stdout=subprocess.PIPE).stdout.read()
	#print_with_type(status)

	status = subprocess.Popen("git status -s", stdout=subprocess.PIPE).stdout.read()
	#print_with_type(status)

	if status == b'':
		print("Working dir clean. Will not commit.")
		return True

	print("You have changes:")
	subprocess.call("git status -s")
	print("Write a commit message, \"ABORT\" to exit or SKIP.")
	print("If this is a false positive, 1) ABORT, 2) commit manually (if necessary) 3) run script again, SKIP")

	message = input()
	if message == "ABORT":
		print("Aborting...")
		return False
	if message == "SKIP":
		print("Skipping...")
		return True

	subprocess.check_call("git add -A", shell=True, stdout=stdout, stderr=stdout)
	subprocess.check_call("git commit -m \"" + message + "\"", shell=True, stdout=stdout, stderr=stdout)

	return True


def print_with_type(obj):
	print(obj, "[of type ", type(obj), "]")


def build_docs(stdout):
	print("Cleaning past docs...")
	subprocess.call("DEL /S /Q __doxygen\html\*.*", shell=True, stdout=stdout, stderr=None)
	subprocess.call("DEL __doxygen\Docs.zip", shell=True, stdout=stdout, stderr=None)

	print("Building new docs...")

	subprocess.check_call("doxygen config", shell=True, cwd="__doxygen", stdout=stdout)
	subprocess.check_call("7z a -tzip __doxygen\Docs.zip __doxygen\html\\", shell=True, stdout=stdout)

	print("Docs built.")


def commit_docs(stdout):
	print("Checking out pages...")
	subprocess.check_call("git checkout gh-pages", stdout=stdout, stderr=stdout)

	print("Copying docs...")
	subprocess.call("DEL /S /Q Docs\\*.*", shell=True, stdout=stdout, stderr=None)
	subprocess.call("mkdir Docs", shell=True, stdout=stdout, stderr=stdout)
	subprocess.check_call("cp -R __doxygen/html Docs/html", shell=True, stdout=stdout)
	subprocess.check_call("cp __doxygen/Docs.zip Docs/Docs.zip", shell=True, stdout=stdout)

	print("Commiting docs...")
	subprocess.check_call("git add -A", shell=True, stdout=stdout, stderr=stdout)
	subprocess.check_call("git commit -m \"Docs updated.\"", shell=True, stdout=stdout, stderr=stdout)

	print("Adding Download Zip link to the docs.")
	subprocess.check_call("git cherry-pick dd434a0", stdout=stdout, stderr=stdout)

	print("Done committing. Checking out master...")
	subprocess.check_call("git checkout master", stdout=stdout)


if __name__ == "__main__":
	main()
