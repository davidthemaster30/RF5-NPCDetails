rm -rf obj
rm -rf bin

dotnet build RF5.HisaCat.NPCDetails.csproj -f net6.0 -c Release

zip -j 'RF5.HisaCat.NPCDetails_v1.5.0.zip' './bin/Release/net6.0/RF5.HisaCat.NPCDetails.dll' './RF5.HisaCat.NPCDetails'
