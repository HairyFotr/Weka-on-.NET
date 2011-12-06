echo "--Building weka.dll"
ikvm\bin\ikvmc -target:library weka.jar
echo "--Now add weka.dll (and ikvm\bin\IKVM.Runtime.dll, ikvm\bin\IKVM.OpenJDK.Core.dll) to your projects References"