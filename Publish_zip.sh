VERSION_PREFIX=0.0.0.2
VERSION_SUFFIX=alpha

FRAMEWORK=netcoreapp3.1

CONFIGURATION=Debug
#CONFIGURATION=Release

mkdir -p publish;
mkdir -p publish/$CONFIGURATION;

for runtime in "win10-x64"; do
	dotnet publish -c $CONFIGURATION -r $runtime --self-contained true;
	
	mkdir -p publish/$CONFIGURATION/$runtime;
	
	for project in "src/AikaEmu.AuthServer" "src/AikaEmu.GameServer" "src/AikaEmu.WebServer"; do
		mkdir -p publish/$CONFIGURATION/$runtime/$project;
		mv $project/bin/$CONFIGURATION/$FRAMEWORK/$runtime/publish/* publish/$CONFIGURATION/$runtime/$project;
		rm -R $project/bin/$CONFIGURATION/$FRAMEWORK/$runtime;
	done;
	
	cd publish/$CONFIGURATION/$runtime;
	7z a -tzip ../../../publish/$CONFIGURATION/AikaEmu.$VERSION_PREFIX-$VERSION_SUFFIX+$runtime.zip *;
	cd ../../../;
	
	rm -R publish/$CONFIGURATION/$runtime;
done;