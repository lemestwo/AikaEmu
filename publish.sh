FRAMEWORK=netcoreapp3.1
CONFIGURATION=Debug
RUNTIME=win10-x64

rm -R bin;
mkdir -p bin;
mkdir -p bin/$CONFIGURATION;

dotnet publish -c $CONFIGURATION -r $RUNTIME --self-contained true;
mkdir -p bin/$CONFIGURATION;
	
for project in "AikaEmu.AuthServer" "AikaEmu.GameServer" "AikaEmu.WebServer"; do
	mkdir -p bin/$CONFIGURATION/$project;
	mv src/$project/bin/$CONFIGURATION/$FRAMEWORK/$RUNTIME/publish/* bin/$CONFIGURATION/$project;
	rm -R src/$project/bin/$CONFIGURATION/$FRAMEWORK/$RUNTIME;
done;