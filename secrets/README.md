## secrets management

Use [sops](https://github.com/mozilla/sops) or [kubesec](https://github.com/shyiko/kubesec) to encrypt secrets and commit it to git.

Both use AWS KMS or any Cloud HSM for key.

Recomend to use mozilla/sops, rather than kubesec.

## mozilla/sops

Root path is `./sops`.

### Prepare rule

You can omit specifing kms everytime by using rule file `.sops`.

```yaml
# creation rules are evaluated sequentially, the first match wins
creation_rules:
  - path_regex: .*/development/secrets/.*.yaml
    kms: "arn:aws:kms:YOUR_KEY_A"
  - path_regex: .*/staging/secrets/secrets\.enc\.yaml$
    kms: "arn:aws:kms:YOUR_KEY_B"
```

### handle secrets

```shell
# generate
sops ./overlay/development/secrets/notexists.enc.yaml 
# edit
sops ./overlay/development/secrets/secrets.enc.yaml 
# encrypt
sops -e ./overlay/development/secrets/secrets.yaml > ./overlay/development/secrets/secrets.enc.yaml
# decrypt
sops -d- secrets.enc.yaml > secrets.yaml
```

## kubesec

Root path is `./kubesec`.

### handle secrets

```shell
# encrypt
kubesec encrypt --key=arn:aws:kms:YOUR_KEY_A ./overlay/development/secrets/secrets.yaml -o ./overlay/development/secrets/secrets.enc.yaml
# decrypt
kubesec decrypt --keyring=arn:aws:kms:YOUR_KEY_B ./overlay/development/secrets/secrets.enc.yaml -o ./overlay/development/secrets/secrets.yaml
```
