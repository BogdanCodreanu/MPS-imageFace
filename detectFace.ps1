param([string]$imagePath)
cd .\FaceDetector\FaceDetector\bin\Debug\net472
./FaceDetector.exe $imagePath
cd ../../../../../