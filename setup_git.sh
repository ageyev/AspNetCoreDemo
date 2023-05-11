# touch .env
# add .env to .gitignore
# add values to .env

source ./.env
echo "GIT_USER_NAME: $GIT_USER_NAME"
echo "GIT_USER_EMAIL: $GIT_USER_EMAIL"

# git init
git config --local user.name "$GIT_USER_NAME"
git config --local user.email "$GIT_USER_EMAIL"
git config --list --local

# add remote repository
# see: https://docs.github.com/en/get-started/getting-started-with-git/managing-remote-repositories
# ls -laFh ~/.ssh/
# ssh-add -D && ssh-add ~/.ssh/<private_key_for_git>