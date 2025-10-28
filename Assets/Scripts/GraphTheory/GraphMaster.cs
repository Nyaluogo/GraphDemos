using UnityEngine;

namespace GraphTheory
{
    public class GraphMaster : MonoBehaviour
    {
        public static GraphMaster Instance { get; private set; }
        GraphMaster() { Instance = this; }
        [System.Serializable]
        public class LinkedListProperties
        {
            public NodeBehavior head = null;
            public NodeBehavior current = null;
            public NodeBehavior tail = null;
            public GameObject dataBodyPrefab;


            public void GenerateLinkedList()
            {
                if(dataBodyPrefab == null)
                {
                    Debug.LogError("Data Body Prefab is not assigned.");
                    return;
                }
                GameObject vertex = Instantiate(dataBodyPrefab);
                NodeBehavior node1 = vertex.GetComponent<NodeBehavior>();
                node1.nodeName = "Node 1";
                node1 = head;
                //print head info

                //start from beginning
                while(node1 != null)
                {
                    node1.DisplayInfo();
                    node1 = node1.nextNode;
                }
            }

            public void InsertAtBeginning(string name)
            {
                if (dataBodyPrefab == null)
                {
                    Debug.LogError("Data Body Prefab is not assigned.");
                    return;
                }
                GameObject vertex = Instantiate(dataBodyPrefab);
                NodeBehavior newNode = vertex.GetComponent<NodeBehavior>();
                if (head == null)
                {
                    head = newNode;
                    tail = newNode;
                }
                else
                {
                    newNode.nextNode = head;
                    head.previousNode = newNode;
                    head = newNode;
                }
            }

            public NodeBehavior InsertAtEnd(string name)
            {
                if (dataBodyPrefab == null)
                {
                    Debug.LogError("Data Body Prefab is not assigned.");
                    return null;
                }
                GameObject vertex = Instantiate(dataBodyPrefab);
                NodeBehavior newNode = vertex.GetComponent<NodeBehavior>();

                newNode.nodeName = name;
                if (tail == null)
                {
                    head = newNode;
                    tail = newNode;
                }
                else
                {
                    tail.nextNode = newNode;
                    newNode.previousNode = tail;
                    tail = newNode;
                }

                if(newNode != null)
                {
                    return newNode;
                }
                else
                {
                    return null;
                }
            }

            public void InsertAfterNode(NodeBehavior previousNode, string name)
            {
                if (previousNode == null)
                {
                    Debug.LogError("The given previous node cannot be null.");
                    return;
                }

                if (dataBodyPrefab == null)
                {
                    Debug.LogError("Data Body Prefab is not assigned.");
                    return;
                }
                GameObject vertex = Instantiate(dataBodyPrefab);
                NodeBehavior newNode = vertex.GetComponent<NodeBehavior>();

                newNode.nodeName = name;
                newNode.nextNode = previousNode.nextNode;
                previousNode.nextNode = newNode;
                newNode.previousNode = previousNode;
                if (newNode.nextNode != null)
                {
                    newNode.nextNode.previousNode = newNode;
                }
                else
                {
                    tail = newNode; // Update tail if new node is added at the end
                }
            }

            public void InsertBeforeNode(NodeBehavior nextNode, string name)
            {
                if (nextNode == null)
                {
                    Debug.LogError("The given next node cannot be null.");
                    return;
                }

                if (dataBodyPrefab == null)
                {
                    Debug.LogError("Data Body Prefab is not assigned.");
                    return;
                }
                GameObject vertex = Instantiate(dataBodyPrefab);
                NodeBehavior newNode = vertex.GetComponent<NodeBehavior>();

                newNode.nodeName = name;
                newNode.previousNode = nextNode.previousNode;
                nextNode.previousNode = newNode;
                newNode.nextNode = nextNode;
                if (newNode.previousNode != null)
                {
                    newNode.previousNode.nextNode = newNode;
                }
                else
                {
                    head = newNode; // Update head if new node is added at the beginning
                }
            }


            public void DeleteAtBeginning()
            {
                if (head == null)
                {
                    Debug.LogError("The list is empty.");
                    return;
                }
                head = head.nextNode;
                if (head != null)
                {
                    head.previousNode = null;
                }
                else
                {
                    tail = null; // List became empty
                }
            }

            public void DeleteAtEnd()
            {
                if (tail == null)
                {
                    Debug.LogError("The list is empty.");
                    return;
                }
                tail = tail.previousNode;
                if (tail != null)
                {
                    tail.nextNode = null;
                }
                else
                {
                    head = null; // List became empty
                }
            }

            public NodeBehavior RemoveVertex(NodeBehavior node)
            {
                if(head == null)
                {
                    Debug.LogError("The list is empty.");
                    return null;
                }
                NodeBehavior currentNode = head;

                while (currentNode != null)
                {
                    if(currentNode == node)
                    {
                        return DeleteVertexByName(currentNode.name);
                    }
                }

                return null;
            }

            public NodeBehavior DeleteVertexByName(string name)
            {
                if (head == null)
                {
                    Debug.LogError("The list is empty.");
                    return null;
                }
                NodeBehavior currentNode = head;
                while (currentNode != null)
                {
                    if (currentNode.nodeName == name)
                    {
                        NodeBehavior temp = currentNode;
                        if (currentNode.previousNode != null)
                        {
                            currentNode.previousNode.nextNode = currentNode.nextNode;
                        }
                        else
                        {
                            head = currentNode.nextNode; // Update head if needed
                        }

                        if (currentNode.nextNode != null)
                        {
                            currentNode.nextNode.previousNode = currentNode.previousNode;
                        }
                        else
                        {
                            tail = currentNode.previousNode; // Update tail if needed
                        }
                        return temp;
                    }
                    currentNode = currentNode.nextNode;
                }
                Debug.LogError("Node with the given name not found.");
                return null;
            }

            public void DeleteAfterNode(NodeBehavior previousNode)
            {
                if (previousNode == null || previousNode.nextNode == null)
                {
                    Debug.LogError("The given previous node is invalid.");
                    return;
                }
                NodeBehavior nodeToDelete = previousNode.nextNode;
                previousNode.nextNode = nodeToDelete.nextNode;
                if (nodeToDelete.nextNode != null)
                {
                    nodeToDelete.nextNode.previousNode = previousNode;
                }
                else
                {
                    tail = previousNode; // Update tail if needed
                }
            }

            public void DeleteBeforeNode(NodeBehavior nextNode)
            {
                if (nextNode == null || nextNode.previousNode == null)
                {
                    Debug.LogError("The given next node is invalid.");
                    return;
                }
                NodeBehavior nodeToDelete = nextNode.previousNode;
                nextNode.previousNode = nodeToDelete.previousNode;
                if (nodeToDelete.previousNode != null)
                {
                    nodeToDelete.previousNode.nextNode = nextNode;
                }
                else
                {
                    head = nextNode; // Update head if needed
                }
            }

            public void RemoveFromNodeToEnd(NodeBehavior startNode)
            {
                if (startNode == null)
                {
                    Debug.LogError("The given start node is invalid.");
                    return;
                }

                // Capture the node before the start so we can reconnect the remaining list after deletion.
                NodeBehavior prev = startNode.previousNode;
                string startName = startNode.nodeName;

                int removedCount = 0;
                NodeBehavior node = startNode;

                // Iterate from the start node to the end, destroy each node GameObject and clear references.
                while (node != null)
                {
                    NodeBehavior next = node.nextNode;

                    // Clear links to avoid accidental dangling references while destroyed.
                    node.previousNode = null;
                    node.nextNode = null;

                    // Destroy the GameObject that holds the NodeBehavior.
                    if (node.gameObject != null)
                        Object.Destroy(node.gameObject);

                    removedCount++;
                    node = next;
                }

                // Reconnect the remaining list (nodes before startNode).
                if (prev != null)
                {
                    prev.nextNode = null;
                    tail = prev;
                }
                else
                {
                    // startNode was head, list is now empty
                    head = null;
                    tail = null;
                }

                Debug.Log($"Removed {removedCount} node(s) starting at '{startName}'.");
            }

            public void ReverseList()
            {
                NodeBehavior currentNode = head;
                NodeBehavior temp = null;
                while (currentNode != null)
                {
                    // Swap next and previous pointers
                    temp = currentNode.previousNode;
                    currentNode.previousNode = currentNode.nextNode;
                    currentNode.nextNode = temp;
                    // Move to the next node (which is previous before swapping)
                    currentNode = currentNode.previousNode;
                }
                // Swap head and tail
                if (temp != null)
                {
                    head = temp.previousNode;
                }
            }

            public int SearchNode(string name)
            {
                NodeBehavior currentNode = head;
                int position = 0;
                while (currentNode != null)
                {
                    if (currentNode.nodeName == name)
                    {
                        return position;
                    }
                    currentNode = currentNode.nextNode;
                    position++;
                }
                return -1; // Node not found
            }

            public int CountNodes()
            {
                NodeBehavior currentNode = head;
                int count = 0;
                while (currentNode != null)
                {
                    count++;
                    currentNode = currentNode.nextNode;
                }
                return count;
            }
        }

        [System.Serializable]
        public class StackProperties
        {
            public const int maxNodesToSpawn = 100;
            public NodeBehavior[] stack = new NodeBehavior[maxNodesToSpawn];
            public int top = -1;
            public void Push(NodeBehavior value)
            {
                // Implementation for pushing a value onto the stack
                stack[++top] = value;
            }
            public NodeBehavior Pop()
            {
                // Implementation for popping a value from the stack
                return stack[top--]; // Placeholder return
            }
            public NodeBehavior Peek()
            {
                // Implementation for peeking at the top value of the stack
                return stack[top]; // Placeholder return
            }
            public bool IsStackEmpty()
            {
                // Implementation to check if the stack is empty
                return top == -1; // Placeholder return
            }
        }

        [System.Serializable]
        public class QueueProperties
        {
            public const int maxNodesToSpawn = 100;
            public NodeBehavior[] queue = new NodeBehavior[maxNodesToSpawn];
            int front = 0;
            int rear = -1;
            public int queueSize = 0;
            public void Insert(NodeBehavior value)
            {
                if (value == null)
                {
                    Debug.LogWarning("Queue.Insert: cannot insert null value.");
                    return;
                }

                if (IsQueueFull())
                {
                    Debug.LogWarning("Queue.Insert: queue is full, cannot insert.");
                    return;
                }

                rear = (rear + 1) % maxNodesToSpawn;
                queue[rear] = value;
                queueSize++;
            }
            public NodeBehavior Remove()
            {
                if (IsQueueEmpty())
                {
                    Debug.LogWarning("Queue.Remove: queue is empty.");
                    return null;
                }

                NodeBehavior value = queue[front];
                queue[front] = null; // clear reference to allow GC / avoid stale refs
                front = (front + 1) % maxNodesToSpawn;
                queueSize--;

                // reset indices when queue becomes empty to keep state clean
                if (queueSize == 0)
                {
                    front = 0;
                    rear = -1;
                }

                return value;
            }

            public NodeBehavior Peek()
            {
                if (IsQueueEmpty()) return null;
                return queue[front];
            }

            // Clear queue quickly
            public void Clear()
            {
                if (queueSize == 0) return;
                // null out used slots
                for (int i = 0; i < queueSize; i++)
                {
                    int idx = (front + i) % maxNodesToSpawn;
                    queue[idx] = null;
                }
                front = 0;
                rear = -1;
                queueSize = 0;
            }

            // Return a snapshot array (front..rear order). O(n)
            public NodeBehavior[] ToArray()
            {
                NodeBehavior[] arr = new NodeBehavior[queueSize];
                for (int i = 0; i < queueSize; i++)
                {
                    arr[i] = queue[(front + i) % maxNodesToSpawn];
                }
                return arr;
            }

            // Rotate front element to rear in O(1) without extra Remove+Insert calls.
            // Useful for SwitchTurn semantics (move front to back).
            public void RotateOne()
            {
                if (queueSize <= 1) return;

                // place a copy/reference of front at new rear position
                rear = (rear + 1) % maxNodesToSpawn;
                queue[rear] = queue[front];
                queue[front] = null;
                front = (front + 1) % maxNodesToSpawn;
                // queueSize unchanged
            }

            public bool IsQueueEmpty()
            {
                // Implementation to check if the queue is empty
                return queueSize == 0;
            }

            public bool IsQueueFull()
            {
                // Implementation to check if the queue is full
                return queueSize == maxNodesToSpawn;
            }
        }

        [System.Serializable]
        public class HeapProperties
        {
            public enum HeapType
            {
                MaxHeap,
                MinHeap
            }
            public HeapType heapType = HeapType.MaxHeap;

            public const int maxNodesToSpawn = 100;
            public NodeBehavior[] heapNodes = new NodeBehavior[maxNodesToSpawn];
            public int heapSize = 0;
            public NodeBehavior rootNode;

            /// <summary>
            /// Gets the index of the parent node. Parent(i) = floor((i - 1) / 2)
            /// </summary>
            private int Parent(int i)
            {
                return (i - 1) / 2;
            }

            /// <summary>
            /// Gets the index of the left child. Left(i) = 2i + 1
            /// </summary>
            private int Left(int i)
            {
                return 2 * i + 1;
            }

            /// <summary>
            /// Gets the index of the right child. Right(i) = 2i + 2
            /// </summary>
            private int Right(int i)
            {
                return 2 * i + 2;
            }

            /// <summary>
            /// Swaps two NodeBehavior elements in the heapNodes array.
            /// </summary>
            private void Swap(int i, int j)
            {
                NodeBehavior temp = heapNodes[i];
                heapNodes[i] = heapNodes[j];
                heapNodes[j] = temp;
            }

            public bool IsHeapEmpty()
            {
                return heapSize == 0;
            }


            // --- Core Heap Operations ---

            /// <summary>
            /// The core function to maintain the max-heap property (sift-down).
            /// Restores the Max-Heap property starting from the given index 'i'.
            /// </summary>
            /// <param name="i">The index of the node to check and fix.</param>
            public void MaxHeapify(int i)
            {
                int left = Left(i);
                int right = Right(i);
                int largest = i;

                // 1. Check if the left child exists and is greater than the current node.
                if (left < heapSize && heapNodes[left].priority > heapNodes[i].priority)
                {
                    largest = left;
                }

                // 2. Check if the right child exists and is greater than the current largest.
                if (right < heapSize && heapNodes[right].priority > heapNodes[largest].priority)
                {
                    largest = right;
                }

                // 3. If the largest element is not the current node, swap them and recursively call MaxHeapify on the affected subtree.
                if (largest != i)
                {
                    Swap(i, largest);
                    MaxHeapify(largest);
                }
            }

            /// <summary>
            /// A general Heapify method that defaults to MaxHeapify(0) to fix a heap from the root.
            /// </summary>
            public void Heapify()
            {
                if (heapSize > 0)
                {
                    MaxHeapify(0);
                }
            }

            /// <summary>
            /// Rearranges the heapNodes array into a Max-Heap structure efficiently.
            /// It starts from the last non-leaf node and works backwards to the root.
            /// </summary>
            public void BuildMaxHeap()
            {
                // For a clean build, we assume the initial 'heapSize' is the number of elements 
                // that have been populated in the array, up to maxNodesToSpawn.
                // The original prompt used `heapNodes.Length`, so we'll enforce the current `heapSize`.

                // The first index of a non-leaf node is at (heapSize / 2) - 1.
                for (int i = (heapSize / 2) - 1; i >= 0; i--)
                {
                    MaxHeapify(i);
                }
            }

            /// <summary>
            /// Inserts a new node into the max-heap (sift-up).
            /// </summary>
            public void Insert(NodeBehavior node)
            {
                if (heapSize >= maxNodesToSpawn)
                {
                    // Prevent array overflow in a game scenario
                    // In a real Unity app, you might use Debug.LogError here.
                    System.Console.WriteLine("Heap is full. Cannot insert new node.");
                    return;
                }

                // Place the new element at the end and increase size
                int i = heapSize;
                heapNodes[i] = node;
                heapSize++;

                // Restore the max-heap property by "sifting up"
                while (i > 0 && heapNodes[Parent(i)].priority < heapNodes[i].priority)
                {
                    Swap(i, Parent(i));
                    i = Parent(i);
                }
            }

            /// <summary>
            /// Extracts and returns the node with the maximum priority (the root).
            /// </summary>
            public NodeBehavior ExtractMax()
            {
                if (heapSize < 1)
                {
                    System.Console.WriteLine("Heap is empty. Cannot extract max.");
                    return null;
                }

                // 1. Get the max element
                NodeBehavior maxNode = heapNodes[0];

                // 2. Move the last element to the root position
                heapNodes[0] = heapNodes[heapSize - 1];
                // 3. Null out the last slot and decrease size
                heapNodes[heapSize - 1] = null;
                heapSize--;

                // 4. Restore the max-heap property by "sifting down"
                MaxHeapify(0);

                return maxNode;
            }

            public NodeBehavior Peek()
            {
                if (heapSize < 1)
                {
                    return null;
                }
                return heapNodes[0];
            }
        }


        [System.Serializable]
        public class DepthFirstSearch
        {
            public const int maxNodesToSpawn = 100;

            public int[] stack = new int[maxNodesToSpawn];
            public NodeBehavior[] nodes=new NodeBehavior[maxNodesToSpawn];
            public int[,] adjacencyMatrix=new int[maxNodesToSpawn,maxNodesToSpawn];
            public int vertexCount = 10;
            public int top = -1;

            public void Push(int value)
            {
                // Implementation for pushing a value onto the stack
                stack[++top] = value;
            }
            public int Pop()
            {
                // Implementation for popping a value from the stack
                return stack[top--]; // Placeholder return
            }

            public int Peek()
            {
                // Implementation for peeking at the top value of the stack
                return stack[top]; // Placeholder return
            }

            public void AddVertex(string name)
            {
                // Implementation for adding a vertex
                NodeBehavior vertex = new NodeBehavior();
                vertex.nodeName = name;
                vertex.visited = false;

                nodes[vertexCount++] = vertex;
            }

            public void AddEdge(int start, int end)
            {
                // Implementation for adding an edge
                adjacencyMatrix[start, end] = 1;
                adjacencyMatrix[end, start] = 1; // For undirected graph
            }

            public int GetUnvisitedAdjacentVertex(int vertexIndex)
            {
                // Implementation to get an unvisited adjacent vertex
                for (int i = 0; i < vertexCount; i++)
                {
                    if (adjacencyMatrix[vertexIndex, i] == 1 && !nodes[i].visited)
                    {
                        return i;
                    }
                }
                return -1; // No unvisited adjacent vertex found
            }

            public bool IsStackEmpty()
            {
                // Implementation to check if the stack is empty
                return top == -1; // Placeholder return
            }

            public void ResetVisitedFlags()
            {
                for (int i = 0; i < vertexCount; i++)
                {
                    nodes[i].visited = false;
                }
            }

            public void InitAdjacencyMatrix()
            {
                for (int i = 0; i < maxNodesToSpawn; i++)
                {
                    for (int j = 0; j < maxNodesToSpawn; j++)
                    {
                        adjacencyMatrix[i, j] = 0;
                    }
                }
            }

            public void DepthFirstSearchTraversal()
            {
                // Implementation for depth-first search traversal
                nodes[0].visited = true;
                nodes[0].DisplayInfo();
                Push(0);
                while (!IsStackEmpty())
                {
                    int currentVertex = Peek();
                    int adjacentVertex = GetUnvisitedAdjacentVertex(currentVertex);
                    if (adjacentVertex == -1)
                    {
                        Pop();
                    }
                    else
                    {
                        nodes[adjacentVertex].visited = true;
                        nodes[adjacentVertex].DisplayInfo();
                        Push(adjacentVertex);
                    }
                }

                // Reset the visited flags for future traversals
                ResetVisitedFlags();
            }
        }


        [System.Serializable]
        public class BreadthFirstSearch
        {
            public const int maxNodesToSpawn = 100;
            public int[] queue = new int[maxNodesToSpawn];
            int front = 0;
            int rear = -1;
            public int queueSize = 0;
            public NodeBehavior[] listNodes = new NodeBehavior[maxNodesToSpawn];
            public int[,] adjacencyMatrixBFS = new int[maxNodesToSpawn, maxNodesToSpawn];
            public int vertexCountBFS = 10;

            public void Insert(int value)
            {
                // Implementation for inserting a value into the queue
                queue[++rear] = value;
                queueSize++;
            }

            public int Remove()
            {
                // Implementation for removing a value from the queue
                queueSize--;
                return queue[front++];
            }

            public bool IsQueueEmpty()
            {
                // Implementation to check if the queue is empty
                return queueSize == 0;
            }

            public void AddVertexBFS(string name)
            {
                // Implementation for adding a vertex
                NodeBehavior vertex = new NodeBehavior();
                vertex.nodeName = name;
                vertex.visited = false;
                listNodes[vertexCountBFS++] = vertex;
            }

            public void AddEdgeBFS(int start, int end)
            {
                // Implementation for adding an edge
                adjacencyMatrixBFS[start, end] = 1;
                adjacencyMatrixBFS[end, start] = 1; // For undirected graph
            }

            public int GetUnvisitedAdjacentVertexBFS(int vertexIndex)
            {
                // Implementation to get an unvisited adjacent vertex
                for (int i = 0; i < vertexCountBFS; i++)
                {
                    if (adjacencyMatrixBFS[vertexIndex, i] == 1 && !listNodes[i].visited)
                    {
                        return i;
                    }
                }
                return -1; // No unvisited adjacent vertex found
            }

            public void ResetVisitedFlagsBFS()
            {
                for (int i = 0; i < vertexCountBFS; i++)
                {
                    listNodes[i].visited = false;
                }
            }

            public void InitAdjacencyMatrixBFS()
            {
                for (int i = 0; i < maxNodesToSpawn; i++)
                {
                    for (int j = 0; j < maxNodesToSpawn; j++)
                    {
                        adjacencyMatrixBFS[i, j] = 0;
                    }
                }
            }

            public void BreadthFirstSearchTraversal()
            {
                // Implementation for breadth-first search traversal
                listNodes[0].visited = true;
                listNodes[0].DisplayInfo();
                Insert(0);
                while (!IsQueueEmpty())
                {
                    int currentVertex = Remove();
                    int adjacentVertex;
                    while ((adjacentVertex = GetUnvisitedAdjacentVertexBFS(currentVertex)) != -1)
                    {
                        listNodes[adjacentVertex].visited = true;
                        listNodes[adjacentVertex].DisplayInfo();
                        Insert(adjacentVertex);
                    }
                }
                // Reset the visited flags for future traversals
                ResetVisitedFlagsBFS();
            }
        }

        

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            Instance = this;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
