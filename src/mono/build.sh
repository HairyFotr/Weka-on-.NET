#/bin/bash
if [ ! -e "weka.dll" ] 
then
	echo "--Building weka.dll"
	ikvmc -target:library weka.jar
else
	echo "--weka.dll already built"
fi
echo "--Building $1.cs" && \
mcs $1.cs -r:weka.dll,/usr/lib/ikvm/IKVM.OpenJDK.Core.dll,/usr/lib/ikvm/IKVM.Runtime.dll && \
echo "--Running $1.exe" && \
mono $1.exe 
