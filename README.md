# Project-Cornflakes

## Pushing to the repository

To make it easier to deal with multiple users, pushing to branch `master` is not allowed. Any commits to be merged with master will be done so by using github's `Pull request` feature. Remember to always write a message with your commits and pull requests.

"Well kim", you say, "How am I supposed to add my code to the repository?"

There are a couple of ways:

### Creating a fork of the repository

Using github you can create a fork of the repository and do whatever you want with that fork.

Keep in mind it will be more difficult to see what others are working on as you will be in your own bubble.

If you fork and work on something while a branch for that part of the project is active on the main repository, you may run into trouble getting your pull request accepted.

### Creating a branch in the main repository

Git has a command for creating branches:
```
git branch [branchname]
```
To go to that branch:
```
git checkout [branchname]
```
And finally, to push that branch to the repository:
```
git push origin [branchname]
```

### "How do i even get the files to my pc?"

This isnt a git tutorial. If something odd comes up or you want to do something outside the box, please let someone know.

Otherwise refer to [github's git cheat sheet](https://github.github.com/training-kit/downloads/github-git-cheat-sheet.pdf) and/or [git-scm's documentation](https://git-scm.com/docs).

### Issues

 * Cannot add/commit lock files
  *  Close all open project files in unity and Visual Studio and try again

### tl;dr

* **Never** commit to `master`
* **Always** write a message when you commit
* **Sometimes** ask for help about git
* **Talk** to other collaborators
* **You** are responsible for what you commit

---