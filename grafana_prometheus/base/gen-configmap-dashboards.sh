#!/bin/bash

# pick script dir
here=$(cd $(dirname $0); pwd)

# dashboard counter
i=1

# increment counter
function incrementCounter() {
  ((i=i+1))
}
# replace space to - for k8s name restriction
function string:replaceSpace() {
  echo "${1}" | sed "s/ /${2}/g"
}
function path:getFiles() {
  (cd "${here}" && ls ${1} | cat)
}
# pick filename w/o extensions
function file:getName() {
  echo "${1%.*}" | sed "s/ /-/g"
}
# add indent
function file:addIndentEachline() {
  cat "${here}/${1}" | sed 's/^/    /g'
}

function genConfigmap() {
  local file="${1}"
  local replaced=$(string:replaceSpace "${file}" "-")
  local name=$(file:getName "${replaced}")
  local json=$(file:addIndentEachline "${file}")
  cat <<EOF > "${here}/configmap-ds${i}.yaml"
apiVersion: v1
kind: ConfigMap
metadata:
  name: grafana-dashboard-${name}
  labels:
    grafana_dashboard: "1"
data:
  ${replaced}: |
${json}
EOF
}

# prepare
ORG_IFS=$IFS
IFS=$(echo -en "\n\b")

# gen
for file in $(path:getFiles "*.json"); do
  genConfigmap "${file}"
  incrementCounter
done

# restore
IFS=$ORG_IFS
