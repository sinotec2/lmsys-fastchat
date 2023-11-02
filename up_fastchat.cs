
env="/nas2/kuang/.conda/envs/YOLOv8"
cd ~/MyPrograms/FastChat
ip=200.200.32.195
addc="http://${ip}:21001"
$env/bin/python3 -m fastchat.serve.controller --host $ip &
mdls=( "lmsys/vicuna-7b-v1.5-16k" "cerebras/btlm-3b-8k-base" )
#"mosaicml/mpt-30b-chat" )
#"jondurbin/airoboros-13b-gpt4-1.4")
port=( 55080 55081)
slps=( 120 480 )
for i in 0 1;do
  add="http://${ip}:${port[$i]}" 
  ~/bin/sub $env/bin/python3 -m fastchat.serve.model_worker --model-path ${mdls[$i]} --host $ip --worker-address $add --controller-address $addc --port ${port[$i]} --device cpu 
  sleep ${slps[$i]}
  done
$env/bin/python3 -m fastchat.serve.gradio_web_server_multi --host $ip --port 55082 --controller-url $addc &

