cd Wiki
git pull
cd ..

git clone https://github.com/gregsdennis/AutoWiki.git
cd AutoWiki\AutoWiki
git pull

dotnet run -p AutoWiki.csproj ..\..\Wiki

goto done

cd ..\..\Wiki
git add -A
git commit -m "updated after build"
git push

cd ..

:done
cd ..\..

rd /s /q AutoWiki