1. Create root app and test accounts GET endpoint
2. Enable istio sidecar 
3. Enable account Peer Auth



istioctl proxy-config routes $(kubectl get pods -n accounts -l app.kubernetes.io/instance=accounts -o jsonpath='{.items[0].metadata.name}') -n accounts -o json

istioctl proxy-config routes $(kubectl get pods -n istio-system -l istio=ingressgateway -o jsonpath='{.items[0].metadata.name}') -n istio-system -o json

istioctl x describe pod $(kubectl get pod -n accounts -l app.kubernetes.io/instance=accounts -o jsonpath='{.items[0].metadata.name}').accounts

kubectl logs $(kubectl get pod -n accounts -l app.kubernetes.io/instance=accounts -o jsonpath='{.items[0].metadata.name}') -n accounts -c istio-proxy