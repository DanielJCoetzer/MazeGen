Maze Generation Algorithm
Daniel Coetzer
I chose to work on the maze generation topic for my final project on AI Game Programming since it could give me more insight into developing random and procedural generation of structured environments within games.

I started out by studying the different maze generation algorithms that were mentioned in the recommended slideshow. The “growing tree” algorithm stood out to me to be the most versatile and fun to experiment with, so I decided to use it for my project. 

For the implementation, I followed this webpage as a guide to understand how the algorithm is supposed to function. To simply explain my algorithm, it contains a list of cells that initially only has one random cell. Iteratively, the algorithm chooses a cell from the list (depending on the console input), and that cell looks for a neighbour cell that hasn’t been discovered yet. If the current cell does not have any neighbours, then it gets removed from the list of cells. The algorithm ends when the entire list is empty. The maze looks a lot different depending on what cell is chosen from the list in the algorithm.

<img width="535" alt="Screenshot 2025-02-11 at 22 44 27" src="https://github.com/user-attachments/assets/df4fa9b4-37bf-4b17-9eb1-0039edee0cd4" />

I then implemented a function that draws the maze on the console using pipes and underscores. After the maze is drawn, it randomly chooses an entrance and exit for the maze.


Thanks for reading!

Links:
Video Demo
