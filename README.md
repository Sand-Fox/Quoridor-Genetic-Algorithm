# ProjetQuoridor

## Table of contents

* [Introduction](#introduction)
* [General Info](#general-info)
* [Setup](#setup)
* [Features](#features)
## Introduction
In perspective of Sorbonne University PIMA classes, we must develop a game named Quoridor.

Quoridor is a 9x9 plate game opposing 2 or 4 players. We will considerate the 2 players variante.
The objective of the game is to reach the other side of the board before your opponent reach your's. 
Each turn you can whether move or place a wall between two case to block your opponent in his path.
It is important to notice you cannot "close" your opponent path, there must always be a path from one side to the opposit one.

Using C# via the cross platform game engine Unity we developted a nice looking and fluid game.
An unfinished version of the game in python is findable on Henri's github.


## General info
This project simulate the game of Quoridor. Implemented by [Henri Besancenot](https://github.com/BlackH57), [Zhichun Hua](https://github.com/ZhicoH) and [Nino Sandlarz](https://github.com/Sand-Fox
).
	

## Setup
To run this project, choose and run one of the builds above (the mac version may not work).

## Features
* Play a game of Quoridor with a friend on server.
* Play against various AI with different behaviours.
* Organize match between AI to determine which moves are the best

* A* pathfinding algorithm to help you in your moves.
* Genetic algorithm to determine the IA with the best parameters.
* Save and load game(s).
