1. Disable sidecar of accounts 
2. Create argocd root app and test accounts GET endpoint
2. Enable accounts sidecar and show nothing changed from GET point of view
3. Enable account Peer Auth and test clear text is not possible anymore

to test mtls make a call from cluster to account ms it should not work....

kubectl exec -n istio-system $(kubectl get pods -n istio-system -l istio=ingressgateway -o jsonpath='{.items[0].metadata.name}') -- curl -i -s  http://accounts-ms-base.accounts.svc.cluster.local/api/v1/accounts


4. Enable deny all auth policy to test now ingress can not call account anymore from Thunder client
5. Enable ingress auth policy to enable ingree access test it will work from Thunder client



<!-- accounts -->
istioctl proxy-config clusters $(kubectl get pods -n accounts -l app.kubernetes.io/instance=accounts -o jsonpath='{.items[0].metadata.name}') -n accounts -o json
istioctl proxy-config routes $(kubectl get pods -n accounts -l app.kubernetes.io/instance=accounts -o jsonpath='{.items[0].metadata.name}') -n accounts -o json
istioctl proxy-config endoints $(kubectl get pods -n accounts -l app.kubernetes.io/instance=accounts -o jsonpath='{.items[0].metadata.name}') -n accounts -o json
istioctl x describe pod $(kubectl get pod -n accounts -l app.kubernetes.io/instance=accounts -o jsonpath='{.items[0].metadata.name}').accounts
kubectl logs $(kubectl get pod -n accounts -l app.kubernetes.io/instance=accounts -o jsonpath='{.items[0].metadata.name}') -n accounts -c istio-proxy


<!-- limits -->

istioctl proxy-config routes $(kubectl get pods -n limits -l app.kubernetes.io/instance=limits -o jsonpath='{.items[0].metadata.name}') -n limits -o json
istioctl x describe pod $(kubectl get pod -n accounts -l app.kubernetes.io/instance=accounts -o jsonpath='{.items[0].metadata.name}').accounts
kubectl logs $(kubectl get pod -n accounts -l app.kubernetes.io/instance=accounts -o jsonpath='{.items[0].metadata.name}') -n accounts -c istio-proxy


<!-- transfers -->

istioctl proxy-config routes $(kubectl get pods -n transfers -l app.kubernetes.io/instance=transfers -o jsonpath='{.items[0].metadata.name}') -n transfers -o json
istioctl proxy-config endpoints $(kubectl get pods -n transfers -l app.kubernetes.io/instance=transfers -o jsonpath='{.items[0].metadata.name}') -n transfers -o json
istioctl proxy-config clusters $(kubectl get pods -n transfers -l app.kubernetes.io/instance=transfers -o jsonpath='{.items[0].metadata.name}') -n transfers -o json
istioctl proxy-config listeners $(kubectl get pods -n transfers -l app.kubernetes.io/instance=transfers -o jsonpath='{.items[0].metadata.name}') -n transfers -o json
istioctl x describe pod $(kubectl get pod -n transfers -l app.kubernetes.io/instance=transfers -o jsonpath='{.items[0].metadata.name}').transfers
kubectl logs $(kubectl get pod -n transfers -l app.kubernetes.io/instance=transfers -o jsonpath='{.items[0].metadata.name}') -n transfers -c istio-proxy



istioctl proxy-config routes $(kubectl get pods -n istio-system -l istio=ingressgateway -o jsonpath='{.items[0].metadata.name}') -n istio-system -o json





kubectl exec -n istio-system $(kubectl get pods -n istio-system -l istio=ingressgateway -o jsonpath='{.items[0].metadata.name}') -- curl -i -s  http://accounts-ms-base.accounts.svc.cluster.local/api/v1/accounts