# GraphDemos: A GameDev's Guide to Understanding Data Structures

### STOP BINGE-WATCHING BORING TUTORIALS AND LECTURES, START BUILDING.

As a Computer Science student, learning Data Structures and Algorithms can seem like a very abstract and dry topic. The books are wordy, the formulas are scary, and it's hard to see the practical application. I never thought I'd need to use things like queues and heaps until I started working on a realtime strategy game with components that had to work in harmony, at scale. How do you define an NPC's path to efficiently travel to multiple destinations? I had stumbled upon the traveling salesman problem without even realizing it.

This devlog is not a tutorial or a LeetCode interview guide. It's a brief documentation of three mini-games I built to demonstrate how abstract data structures can be used to build simple games.

My goal was simple: **force myself to use the data structures.** No theory lectures, just building.

Here's the plan I followed:
1.  **Snake Game** → `Linked List`
2.  **Card Game** → `Stacks` & `Queues`
3.  **RTS War Game** → `Heaps` (as a Priority Queue)

---

## 1. The Snake & The Linked List

First up: the classic Snake game. This seemed like the most obvious fit for a **Linked List**. The snake's body is a chain of segments, and it's constantly adding to one end (the head) and removing from the other (the tail).

### The Concept vs. The Application

In theory, a Linked List is just a series of nodes where each node points to the next one. My `GraphMaster.cs` file defines this structure, where each `NodeBehavior` has a `nextNode` and `previousNode`.

```csharp name=Assets/Scripts/GraphMaster.cs url=https://github.com/Nyaluogo/GraphDemos/blob/1169c9b291d340b90e9d6c770d10d10c73e04561/Assets/Scripts/GraphMaster.cs
public class NodeBehavior : MonoBehaviour
{
    public NodeBehavior nextNode;
    public NodeBehavior previousNode;
    // ... other properties
}
```

The `LinkedListProperties` class manages the head, tail, and count, making list operations efficient.

```csharp name=Assets/Scripts/GraphMaster.cs url=https://github.com/Nyaluogo/GraphDemos/blob/1169c9b291d340b90e9d6c770d10d10c73e04561/Assets/Scripts/GraphMaster.cs
[System.Serializable]
public class LinkedListProperties
{
    public NodeBehavior head;
    public NodeBehavior tail;
    public int nodeCount;

    // ... Methods for InsertAtEnd, InsertAtBeginning, Remove, etc.
}
```

### The "Aha!" Moment: Moving the Snake

The real lightbulb moment came with the `MoveSnake()` function. I'd always thought moving a snake meant "move the head, then have every single body part take the position of the one in front of it." Implementing this with a linked list felt so natural.

My `MoveSnake()` function in `Snake_LinkedList.cs` shows this perfectly. It's a chain reaction: move the head, then pass the head's old position down the chain.

```csharp name=Assets/Scripts/Snake_LinkedList.cs url=https://github.com/Nyaluogo/GraphDemos/blob/1169c9b291d340b90e9d6c770d10d10c73e04561/Assets/Scripts/Snake_LinkedList.cs
public void MoveSnake()
{
    if (snakeBody.head == null) return;

    Vector3 previousPosition = snakeBody.head.transform.position;
    snakeBody.head.transform.position += moveDirection;

    NodeBehavior currentNode = snakeBody.head.nextNode;
    while (currentNode != null)
    {
        Vector3 tempPosition = currentNode.transform.position;
        currentNode.transform.position = previousPosition;
        previousPosition = tempPosition;
        currentNode = currentNode.nextNode;
    }
}
```

Growing was even easier. The `Grow()` function just calls `snakeBody.InsertAtEnd(node_name)`, which is a simple O(1) operation because my `LinkedListProperties` tracks the tail. No array resizing, no `List.Insert()`. Just pure node-pointer magic.

*[Insert YouTube video snippet here]*

---

## 2. Card Game & Stacks/Queues

Next, I built a card game. This was the classic textbook example. A deck of cards is the perfect analogy for a **Stack** (Last-In, First-Out). And managing player turns felt like a good fit for a **Queue** (First-In, First-Out).

### The Concept vs. The Application

**Stacks (LIFO):** My `GraphMaster.cs` defines a `StackProperties` class. It's a simple array with a `top` index.

```csharp name=Assets/Scripts/GraphMaster.cs url=https://github.com/Nyaluogo/GraphDemos/blob/1169c9b291d340b90e9d6c770d10d10c73e04561/Assets/Scripts/GraphMaster.cs
[System.Serializable]
public class StackProperties
{
    public NodeBehavior[] stack;
    public int top;
    public int maxSize;

    public void Push(NodeBehavior node) { /*...*/ }
    public NodeBehavior Pop() { /*...*/ }
    // ... other methods
}
```

My `Card_Stacks.cs` script uses this for everything:
*   `LoadDeck()`: `Deck.Push(cardVertex)` to fill the initial deck.
*   `DrawCard()`: `NodeBehavior drawnCard = drawPile.Pop()` to take from the top.
*   `DiscardCard()`: `DiscardPile.Push(target)` to add to the top.

It worked exactly as advertised. It was simple, fast, and intuitive.

**Queues (FIFO):** The more interesting part was the turn manager. `CardRules.cs` uses a `QueueProperties` (from `GraphMaster.cs`) called `playingTurnQueue`.

### The "Aha!" Moment: RotateOne()

When setting up the game, I just `Insert()` each player into the `playingTurnQueue`. The player at the front of the queue is the current player. My first instinct for `SwitchTurn()` was to `Remove()` from the front and `Insert()` to the back.

But then I saw a cooler way. Since my queue was a circular array, I implemented a `RotateOne()` method. This made my `SwitchTurn()` function in `CardRules.cs` a beautifully simple, O(1) operation:

```csharp name=Assets/Scripts/CardRules.cs url=https://github.com/Nyaluogo/GraphDemos/blob/1169c9b291d340b90e9d6c770d10d10c73e04561/Assets/Scripts/CardRules.cs
public void SwitchTurn()
{
    playingTurnQueue.RotateOne();
    currentPlayer = playingTurnQueue.Peek();
    Debug.Log("Next Turn: " + currentPlayer.name);
}
```
This felt so much more efficient than a `Remove` followed by an `Insert`.

*[Insert YouTube video snippet here]*

---

## 3. RTS War Game & The Heap

This was the big one. I wanted to make a simple RTS where soldiers fight, but higher-ranking soldiers get to act more often. This screams **Priority Queue**, and the best way to build a Priority Queue is with a **Heap**.

### The Concept vs. The Application

A Heap is a tree-based structure where every parent node has a higher priority than its children. This means the root node is always the highest-priority item.

My `HeapProperties` in `GraphMaster.cs` is implemented as a Priority Heap. The `ExtractMax` function (which should probably be named `ExtractMin`) pulls the node with the lowest priority number—which is exactly what I want, since `General = 0` is the highest rank.

The core of the heap is the `Insert` (sift-up) and `ExtractMax` (sift-down) logic. `ExtractMax` is the most critical:

```csharp name=Assets/Scripts/GraphMaster.cs url=https://github.com/Nyaluogo/GraphDemos/blob/1169c9b291d340b90e9d6c770d10d10c73e04561/Assets/Scripts/GraphMaster.cs
public NodeBehavior ExtractMax()
{
    if (heapSize < 1)
    {
        Debug.LogError("Heap underflow");
        return null;
    }
    NodeBehavior max = heap[0];
    heap[0] = heap[heapSize - 1];
    heapSize--;
    MaxHeapify(0);
    return max;
}

void MaxHeapify(int i)
{
    int l = 2 * i + 1;
    int r = 2 * i + 2;
    int largest = i;

    if (l < heapSize && heap[l].GetComponent<BattleSoldier>().priority < heap[largest].GetComponent<BattleSoldier>().priority)
    {
        largest = l;
    }
    if (r < heapSize && heap[r].GetComponent<BattleSoldier>().priority < heap[largest].GetComponent<BattleSoldier>().priority)
    {
        largest = r;
    }

    if (largest != i)
    {
        // Swap
        NodeBehavior temp = heap[i];
        heap[i] = heap[largest];
        heap[largest] = temp;

        MaxHeapify(largest);
    }
}
```

### The "Aha!" Moment: The Game Loop

The entire RTS battle is driven by this heap in `BattleHeap_Rules.cs`. Each faction has a `playerCommandQueue` (a `HeapProperties`). The main game loop in `ProcessBattleTick()` is where it all comes together:

```csharp name=Assets/Scripts/BattleHeap_Rules.cs url=https://github.com/Nyaluogo/GraphDemos/blob/1169c9b291d340b90e9d6c770d10d10c73e04561/Assets/Scripts/BattleHeap_Rules.cs
void ProcessBattleTick()
{
    foreach (var faction in factions)
    {
        if (faction.playerCommandQueue.heapSize > 0)
        {
            NodeBehavior actingNode = faction.playerCommandQueue.ExtractMax();
            if (actingNode != null)
            {
                BattleSoldier soldier = actingNode.GetComponent<BattleSoldier>();
                if (soldier != null)
                {
                    soldier.ExecuteSoldierTurn();
                    faction.playerCommandQueue.Insert(actingNode); // Re-queue the soldier
                }
            }
        }
    }
}
```

1. It iterates through each faction.
2. It calls `faction.playerCommandQueue.ExtractMax()` to get the highest-priority soldier.
3. That soldier executes their turn (`ExecuteSoldierTurn()`).
4. Then—and this is the key—it calls `faction.playerCommandQueue.Insert(actingNode)` to put the soldier back in the queue for their next turn.

*[Insert YouTube video snippet here]*

---

## Conclusion

Building these games took data structures from "abstract textbook theory" to "practical tools I can't live without."

- The Snake **needed** a **Linked List** to feel fluid.
- The Card Game **needed** **Stacks** and **Queues** to be functional.
- The RTS **needed** a **Heap** to feel strategic.

If you're struggling with DSA as a gamedev, my advice is to **stop reading and start building**. Pick a simple game. You'll quickly find that you don't just *want* to use a data structure, you *have* to. And that's when you'll finally understand it.

You can find the full devlog videos **[here]** and the source code for all three games **[here on GitHub]**.
