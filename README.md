# MPS-imageFace

To detect the face data, run the powershell script detectFace.ps1.

The script uses the path to the image file as first argument.

The output of the script is given in Rectangles, each rectangle being represented in 4 int values: xCoord, yCoord, width, height. Every value represents image pixel dimensions.

The face detector will also create another image in the same directory as the input image with the detected data being highlighted.

Example of run:


*PS E:\mps tema 2\MPS-imageFace> .\detectFace.ps1 "C:\Users\X\Desktop\imgtest1.jpg"*

*246 115 308 308 424 200 72 72 325 209 68 68*
