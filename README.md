# AllocationMethods
Graduate group project for COSC 519 class at Towson University, MD. Project was to create visual simulation of disk file allocation methods. The project utilizes a MVVM structure facilitated by the MVVM Toolkit(http://www.mvvmlight.net/) and a metro theme which takes advantage of the Mahapps Metro Theme (http://mahapps.com/) 

## Contiguous Method ##
Stores will only fill blocks where the file can be contained in contiguous blocks. Segmentation is obvious after running for longer periods of time.
![Alt text](Screenshot1.png?raw=true "Contiguous Method")

## Linked Method ##
Files can be linked across blocks. The mouseover in the disk is helpful to identify the next linked blocks. The file with an arrow indicates the block is linked.
![Alt text](Screenshot2.png?raw=true "Linked Method")

## Indexed Method##
A single index block for the file contains all the links. Mouse over blocks marked with the bookmark to see a list the file's indexed blocks.
![Alt text](Screenshot3.png?raw=true "Indexed Method")
