1. Disable sidecar of accounts 
2. Create argocd root app and test accounts GET endpoint
2. Enable accounts sidecar and show nothing changed from GET point of view
3. Enable account Peer Auth and test clear text is not possible anymore

to test mtls make a call from cluster to account ms it should not work....

kubectl exec -n istio-system $(kubectl get pods -n istio-system -l istio=ingressgateway -o jsonpath='{.items[0].metadata.name}') -- curl -i -s  http://accounts-ms-base.accounts.svc.cluster.local/api/v1/accounts


4. Enable deny all auth policy to test now ingress can not call account anymore from Thunder client
5. Enable ingress auth policy to enable ingree access test it will work from Thunder client





<!-- accounts -->
istioctl pc endpoints deploy/istio-ingressgateway.istio-system --cluster 'outbound|80||accounts-ms-base.accounts.svc.cluster.local' -o json
istioctl pc endpoints svc/accounts-ms-base -n accounts 
istioctl pc clusters svc/accounts-ms-base -n accounts 
istioctl pc listeners svc/accounts-ms-base -n accounts 
istioctl x describe pod $(kubectl get pod -n accounts -l app.kubernetes.io/instance=accounts -o jsonpath='{.items[0].metadata.name}').accounts


<!-- limits -->

istioctl pc endpoints svc/limits-ms-base -n limits 
istioctl pc clusters svc/limits-ms-base -n limits 
istioctl pc listeners svc/limits-ms-base -n limits 
istioctl x describe pod $(kubectl get pod -n limits -l app.kubernetes.io/instance=limits -o jsonpath='{.items[0].metadata.name}').limits


<!-- transfers -->
istioctl pc endpoints deploy/istio-ingressgateway.istio-system --cluster 'outbound|80||transfers-ms-base.transfers.svc.cluster.local' -o json
istioctl pc endpoints svc/transfers-ms-base -n transfers 
istioctl pc clusters svc/transfers-ms-base -n transfers 
istioctl pc listeners svc/transfers-ms-base -n transfers 
istioctl x describe pod $(kubectl get pod -n transfers -l app.kubernetes.io/instance=transfers -o jsonpath='{.items[0].metadata.name}').transfers


<!-- istioctl proxy-config endpoints $(kubectl get pods -n transfers -l app.kubernetes.io/instance=transfers -o jsonpath='{.items[0].metadata.name}') -n transfers -o json
istioctl proxy-config clusters $(kubectl get pods -n transfers -l app.kubernetes.io/instance=transfers -o jsonpath='{.items[0].metadata.name}') -n transfers -o json
istioctl proxy-config listeners $(kubectl get pods -n transfers -l app.kubernetes.io/instance=transfers -o jsonpath='{.items[0].metadata.name}') -n transfers -o json
istioctl x describe pod $(kubectl get pod -n transfers -l app.kubernetes.io/instance=transfers -o jsonpath='{.items[0].metadata.name}').transfers
kubectl logs $(kubectl get pod -n transfers -l app.kubernetes.io/instance=transfers -o jsonpath='{.items[0].metadata.name}') -n transfers -c istio-proxy -->



istioctl proxy-config routes $(kubectl get pods -n istio-system -l istio=ingressgateway -o jsonpath='{.items[0].metadata.name}') -n istio-system -o json





kubectl exec -n istio-system $(kubectl get pods -n istio-system -l istio=ingressgateway -o jsonpath='{.items[0].metadata.name}') -- curl -i -s  http://accounts-ms-base.accounts.svc.cluster.local/api/v1/accounts