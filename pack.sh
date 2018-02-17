cd Origind.Card.Game/bin

mkdir CQP

cd CQP
mkdir app
mkdir Origind.Card.Game

cd app
cp ../../../NewbeLibs/Platform/CQP/Native/Newbe.Mahua.CQP.Native.dll .
mv Newbe.Mahua.CQP.Native.dll Origind.Card.Game.dll
cp ../../../Origind.Card.Game.json .
cd ..

cd Origind.Card.Game
cp -r ../Release/* .
cp ../../NewbeLibs/Platform/CQP/CLR/* .
cd ..

cp ../../NewbeLibs/Framework/* .
cd ..

zip -r release.zip CQP