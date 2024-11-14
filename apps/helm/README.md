1. Disable sidecar of accounts 
2. Create argocd root app and test accounts GET endpoint
2. Enable accounts sidecar and show nothing changed from GET point of view
3. Enable account Peer Auth and test clear text is not possible anymore

to test mtls make a call from cluster to account ms it should not work....

kubectl exec -n istio-system $(kubectl get pods -n istio-system -l istio=ingressgateway -o jsonpath='{.items[0].metadata.name}') -- curl -i -s  http://accounts-ms-base.accounts.svc.cluster.local/api/v1/accounts


4. Enable account AuthorizationPolicy - miss config ns to show GET account from Thunder client will failt




istioctl proxy-config routes $(kubectl get pods -n accounts -l app.kubernetes.io/instance=accounts -o jsonpath='{.items[0].metadata.name}') -n accounts -o json

istioctl proxy-config routes $(kubectl get pods -n istio-system -l istio=ingressgateway -o jsonpath='{.items[0].metadata.name}') -n istio-system -o json

istioctl x describe pod $(kubectl get pod -n accounts -l app.kubernetes.io/instance=accounts -o jsonpath='{.items[0].metadata.name}').accounts

kubectl logs $(kubectl get pod -n accounts -l app.kubernetes.io/instance=accounts -o jsonpath='{.items[0].metadata.name}') -n accounts -c istio-proxy

kubectl exec -n istio-system $(kubectl get pods -n istio-system -l istio=ingressgateway -o jsonpath='{.items[0].metadata.name}') -- curl -i -s  http://accounts-ms-base.accounts.svc.cluster.local/api/v1/accounts