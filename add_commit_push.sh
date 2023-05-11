source ./.env
git config --local user.name "$GIT_USER_NAME"
git config --local user.email "$GIT_USER_EMAIL"
ssh-add -D
ssh-add "$PRIVATE_KEY_FOR_GIT"
git add . && git commit -a -m 'update' && git push --all 
