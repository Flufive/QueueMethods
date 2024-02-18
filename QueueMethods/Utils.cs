﻿using Unit4.CollectionsLib;
using System;
using System.IO;
using System.ComponentModel;
using System.Linq;
using System.Collections;

namespace QueueMethods
{
    /// <summary>
    /// Utils.
    /// </summary>
    /// 
    public static class Utils
    {
        /// <summary>
        /// Gets the length of the queue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q">Your queue</param>
        /// <returns>The length of the queue</returns>
        public static int Length<T>(this Queue<T> q)
        {
            if (q == null) return 0;

            int counter = 0; // Counter

            Queue<T> tmp = new(); // Fictive queue

            while (!q.IsEmpty())
            {
                tmp.Insert(q.Remove());
                counter++; // Count up the length
            }

            while (!tmp.IsEmpty())
            {
                q.Insert(tmp.Remove()); // Put back in original queue
            }

            return counter;
        }

        /// <summary>
        /// Turns a range into a queue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="range">Your range</param>
        /// <returns>A queue with all elements in the range</returns>
        public static Queue<T> ToQueue<T>(System.Collections.Generic.IEnumerable<T> range)
        {
            if (range == null)
            {
                return new Queue<T>();
            }

            Queue<T> queue = new Queue<T>();

            foreach (T item in range)
            {
                queue.Insert(item);
            }

            return queue;
        }

        /// <summary>
        /// Enqueues a whole range/collection into the queue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q">Your queue</param>
        /// <param name="range">The collection you want to add</param>
        public static void EnqueueRange<T>(this Queue<T> q, System.Collections.Generic.IEnumerable<T> range)
        {
            foreach (var item in range)
            {
                q.Insert(item);
            }
        }

        /// <summary>
        /// Removes all elements from the queue and returns them in the form of a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q">Your queue</param>
        /// <returns>A list containing all elements of the queue</returns>
        /// <exception cref="ArgumentNullException">Thrown when q is null</exception>
        public static System.Collections.Generic.List<T> DequeueToList<T>(this Queue<T> q)
        {
            ArgumentNullException.ThrowIfNull(q);

            q.Clear();

            System.Collections.Generic.List<T> list = new((System.Collections.Generic.IEnumerable<T>)q);

            return list;
        }

        /// <summary>
        /// Finds whether an element is contained within a queue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q">Your queue</param>
        /// <param name="element">The element you want to find</param>
        /// <exception cref="ArgumentNullException">Thrown when element or q is null</exception>
        /// <returns>True if the element is found in the queue, otherwise false</returns>
        public static bool Contains<T>(this Queue<T> q, T element)
        {
            ArgumentNullException.ThrowIfNull(element);

            ArgumentNullException.ThrowIfNull(q);

            Queue<T> tmp = new(); // Fictive queue

            while (!q.IsEmpty())
            {
                tmp.Insert(q.Remove());
            }

            bool flag = false;

            while (!tmp.IsEmpty()) // If element has been found, no need for more checks
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                if (!flag)
                {
                    flag = tmp.Head().Equals(element); // Element is found, flag is true
                }
                q.Insert(tmp.Remove());
                    
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }

            return flag;
        }

        /// <summary>
        /// Peeks at the element at an index within the queue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q">Your queue</param>
        /// <param name="index">The index you want to look at</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when index is larger than queue length or is negative</exception>
        /// <exception cref="ArgumentNullException">Thrown when q is null</exception>
        public static T PeekAt<T>(this Queue<T> q, int index)
        {
            ArgumentNullException.ThrowIfNull(q);

            if (index > q.Length() || index < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            T? item = default;

            Queue<T> tmp = new(); // Fictive

            while (q.IsEmpty())
            {
                tmp.Insert(q.Remove());
            }

            int counter = 0; // Counter to Compare with index

            while (!tmp.IsEmpty())
            {
                if (counter == index) // Iterate through temp to get to the index
                {
                    item = tmp.Head();
                }

                counter++;
                q.Insert(tmp.Remove());
            }
#pragma warning disable CS8603 // Possible null reference return.
            return item; // Item cannot be null here, as the index is definitely within the bounds of the queue
#pragma warning restore CS8603 // Possible null reference return.
        }

        /// <summary>
        /// Returns the given queue in array form
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q">The queue to convert into an array</param>
        /// <returns>An array form of the queue</returns>
        public static T[] ToArray<T>(this Queue<T> q)
        {
            if (q == null)
            {
                return []; // Queue is empty, return empty array
            }

            T[] ToReturn = new T[q.Length()];

            Queue<T> tmp = new();

            while (!q.IsEmpty())
            {
                tmp.Insert(q.Remove());
            }

            int length = tmp.Length();
            for (int i = 0; i < length; i++)
            {
                ToReturn[i] = tmp.Head(); // Insert temp queue into array
                q.Insert(tmp.Remove());
            }

            return ToReturn;
        }

        /// <summary>
        /// Copies a Queue into a given Array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q">Your queue</param>
        /// <param name="arr">The array to copy to</param>
        /// <param name="index">The index to start at</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when index is outside of the Array, or Array can't fit the whole Queue</exception>
        /// <exception cref="ArgumentNullException">Thrown when either Array or Queue is null</exception>
        public static void CopyTo<T>(this Queue<T> q, T[] arr, int index)
        {
            if (index > arr.Length || index < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            int qLength = q.Length();

            if (arr.Length < qLength + index)
            {
                throw new ArgumentOutOfRangeException("arr");
            }

            ArgumentNullException.ThrowIfNull(q);

            if (q.IsEmpty())
            {
                arr = [];
            }

            Queue<T> tmp = new(); // Fictive

            while (!q.IsEmpty())
            {
                tmp.Insert(q.Remove());
            }

            for (int i = index; i < index + qLength; i++)
            {
                arr[i] = tmp.Head(); // Insert into the array from the index
                q.Insert(tmp.Remove());
            }
        }

        /// <summary>
        /// Removes all elements from the queue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q">Your queue</param>
        public static void Clear<T>(this Queue<T> q)
        {
            if (q == null)
            {
                return;
            }

            while (!q.IsEmpty())
            {
                q.Remove();
            }
        }

        /// <summary>
        /// Gets the last element in a queue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q">Your queue</param>
        /// <returns>The last element in the queue</returns>
        /// <exception cref="ArgumentNullException">Thrown when q is null</exception>
        public static T? GetLast<T>(this Queue<T> q)
        {
            ArgumentNullException.ThrowIfNull(q);

            if (q.IsEmpty())
            {
                return default;
            }

            T? lastElement = default; // Sets default value for the last element

            Queue<T> tmp = new();

            while (!q.IsEmpty())
            {
                tmp.Insert(q.Remove());
            }

            while (!tmp.IsEmpty())
            {
                lastElement = tmp.Head(); // Gets the next element until temp queue is empty (reached last)
                q.Insert(tmp.Remove());
            }

            return lastElement;
        }

        /// <summary>
        /// Removes the last element from the Queue and returns it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q">Your queue</param>
        /// <returns>The last element in the queue</returns>
        /// <exception cref="ArgumentNullException">Thrown when q is null</exception>
        /// <exception cref="ArgumentException">Thrown when q is empty</exception>
        public static T RemoveLast<T>(this Queue<T> q)
        {
            ArgumentNullException.ThrowIfNull(q);

            if (q.IsEmpty())
            {
                throw new ArgumentException("queue cannot be empty", "q");
            }

            Queue<T> tmp = new();

            int length = q.Length();

            while (length-- > 1)
            {
                tmp.Insert(q.Remove()); // Insert all elements into a temp queue until the last one
            }

            T lastElement = q.Remove(); // Removes last element

            while (!tmp.IsEmpty())
            {
                q.Insert(tmp.Remove()); // Re-insert all other elements into the queue
            }

            return lastElement;
        }
        /// <summary>
        /// Reverses the given queue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q">Your queue</param>
        /// <exception cref="ArgumentNullException">Thrown when q is null</exception>
        public static void Reverse<T>(this Queue<T> q)
        {
            ArgumentNullException.ThrowIfNull(q);

            if (q.IsEmpty())
            {
                return;
            }

            Queue<T> tmp = new();

            while (!q.IsEmpty())
            {
                tmp.Insert(RemoveLast(q)); // Add into fictive queue in reverse order
            }

            while (!tmp.IsEmpty())
            {
                q.Insert(tmp.Remove()); // Add back into regular queue
            }
        }

        /// <summary>
        /// Sorts a queue using the default comparer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q">The queue you want to sort (ref keyword)</param>
        public static void Sort<T>(this Queue<T> q)
        {
            System.Collections.Generic.List<T> list = q.DequeueToList();

            list.Sort();

            q = ToQueue(list);
        }

        /// <summary>
        /// Deletes all duplicates from the queue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q">Your queue (ref keyword, obviously)</param>
        /// <exception cref="ArgumentNullException">Thrown when q is null</exception>
        public static void TrimExcess<T>(this Queue<T> q)
        {
            ArgumentNullException.ThrowIfNull(q);

            if (q.IsEmpty())
            {
                return;
            }

            // Create a set to track unique elements encountered
            System.Collections.Generic.HashSet<T> uniqueElements = new();

            // Create a temporary queue to hold non-duplicate elements
            Queue<T> tempQueue = new();

            // Iterate through the original queue
            int length = q.Length();
            while (length-- > 0)
            {
                T currentElement = q.Remove();

                // If the current element is not already encountered, enqueue it to tempQueue
                if (uniqueElements.Add(currentElement))
                {
                    tempQueue.Insert(currentElement);
                }
            }

            // Clear the original queue
            q.Clear();

            // Enqueue elements back to the original queue from tempQueue
            while (!tempQueue.IsEmpty())
            {
                q.Insert(tempQueue.Remove());
            }
        }

        /// <summary>
        /// Removes the element at the given index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q">Your queue</param>
        /// <param name="index">The index you want to remove the element att</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when q is null</exception>
        /// <exception cref="ArgumentException">Thrown when q is empty</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when index is outside the bounds of q</exception>
        public static T RemoveAt<T>(this Queue<T> q, int index)
        {
            ArgumentNullException.ThrowIfNull(q);

            if (q.IsEmpty())
            {
                throw new ArgumentException("The queue cannot be empty", "q");
            }

            int length = q.Length();
            int permLength = length;

            if (index < 0 || index > length)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            Queue<T> tmp = new();

            while (length-- > (permLength - index))
            {
                tmp.Insert(q.Remove()); // Insert all elements into a temp queue until getting to the wanted element
            }

            T ToReturn = q.Remove(); // Removes wanted element

            while (!q.IsEmpty())
            {
                tmp.Insert(q.Remove()); // Insert rest of the elements into the temp queue, in order to put it back in regular queue in order
            }

            while (!tmp.IsEmpty())
            {
                q.Insert(tmp.Remove()); // Re-insert all other elements into the queue
            }

            return ToReturn;
        }

        /// <summary>
        /// Removes an element which matches the given element
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q">Your queue</param>
        /// <param name="value">The value you want to remove</param>
        /// <exception cref="ArgumentNullException">Thrown when q is null</exception>
        /// <returns>True if the value was found and removed, otherwise false</returns>
        public static bool RemoveVal<T>(this Queue<T> q, T value)
        {            
            ArgumentNullException.ThrowIfNull(q);

            if (!q.Contains(value))
            {
                return false;
            }

            Queue<T> tempQueue = new(); // Temporary queue to store non-matching elements

            
            while (!q.IsEmpty()) // Remove occurrences of the specified value from the original queue
            {
                T item = q.Remove();
                if (!System.Collections.Generic.EqualityComparer<T>.Default.Equals(item, value))
                {
                    tempQueue.Insert(item);
                }
            }

            
            while (!tempQueue.IsEmpty()) // Copy elements back to the original queue
            {
                q.Insert(tempQueue.Remove());
            }

            return true;
        }

        /// <summary>
        /// Returns all the elements in parents that the attribute of which match a given value
        /// </summary>
        /// <typeparam name="TParent"></typeparam>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="parents">Queue of parents</param>
        /// <param name="childSelector">The method that returns the given attribute of the parent.
        /// a simple implementation would look like this:
        /// Public static TChild GetChild()
        /// ----return this.Child;
        /// </param>
        /// <param name="value">The value</param>
        /// <exception cref="ArgumentNullException">Thrown if either the parents queue or the value are null</exception>
        /// <returns>An IEnumerable<typeparamref name="TParent"/> which contains all the elements that the attribute of which match the given value</returns>
        public static System.Collections.Generic.List<TParent> MatchesVal<TParent, TChild>(Queue<TParent> parents, 
            Func<TParent, TChild> childSelector,
        TChild value)
        {
            ArgumentNullException.ThrowIfNull(parents);
            ArgumentNullException.ThrowIfNull(value);

            if (parents.IsEmpty())
            {
                return [];
            }

            System.Collections.Generic.List<TParent> newParents = new((System.Collections.Generic.IEnumerable<TParent>)parents);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return newParents.Where(parent => childSelector(parent).Equals(value)).ToList(); // CBA to make sure it isnt null honestly
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        /// <summary>
        /// Adds a given element into the queue in a given index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q">Your queue</param>
        /// <param name="index">The index you want to add to</param>
        /// <param name="value">Your element</param>
        /// <exception cref="ArgumentNullException">Thrown when q is null</exception>
        /// <exception cref="ArgumentException">Thrown when q is empty</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when index is outside of the bounds of q</exception>
        public static void AddAt<T>(this Queue<T> q, int index, T value)
        {
            ArgumentNullException.ThrowIfNull(q);

            if (q.IsEmpty())
            {
                throw new ArgumentException("The queue cannot be empty", "q");
            }

            int length = q.Length();
            int permLength = length;

            if (index < 0 || index > length)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            Queue<T> tmp = new();

            while (length-- > (permLength - index))
            {
                tmp.Insert(q.Remove()); // Insert all elements into a temp queue until getting to the wanted element
            }

            tmp.Insert(value); // Add the wanted element

            while (!q.IsEmpty())
            {
                tmp.Insert(q.Remove()); // Insert rest of the elements into the temp queue, in order to put it back in regular queue in order
            }

            while (!tmp.IsEmpty())
            {
                q.Insert(tmp.Remove()); // Re-insert all other elements into the queue
            }
        }

        /// <summary>
        /// Merges 2 queues together into one
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q1">The first queue</param>
        /// <param name="q2">The second queue</param>
        /// <returns>A new queue composed of the first and second queues</returns>
        /// <exception cref="ArgumentNullException">Thrown when either q1 or q2 is null</exception>
        public static Queue<T> Merge<T>(this Queue<T> q1, Queue<T> q2)
        {
            ArgumentNullException.ThrowIfNull(q1);
            ArgumentNullException.ThrowIfNull(q2);

            if (q1.IsEmpty())
            {
                return q2;
            }

            if (q2.IsEmpty())
            {
                return q1;
            }

            Queue<T> ToReturn = new();

            System.Collections.Generic.List<T> tmp1 = DequeueToList(q1);
            System.Collections.Generic.List<T> tmp2 = DequeueToList(q2);

            foreach (var item in tmp1)
            {
                ToReturn.Insert(item);
                q1.Insert(item);
            }

            foreach (var item in tmp2)
            {
                ToReturn.Insert(item);
                q2.Insert(item);
            }

            return ToReturn;
        }

        /// <summary>
        /// Returns a list from the specified index at the length of count
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q">Your queue</param>
        /// <param name="index">Starting index</param>
        /// <param name="count">Length of range</param>
        /// <returns>A range (list) starting at index, with the length of {count}</returns>
        /// <exception cref="ArgumentNullException">Thrown when q is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when index is outside of the bounds of q</exception>
        /// <exception cref="ArgumentException">Thrown when count is either non-positive, or too large in regards to index</exception>
        public static System.Collections.Generic.List<T> GetRange<T>(this Queue<T> q, int index, int count)
        {
            ArgumentNullException.ThrowIfNull(q);
            if (q.IsEmpty())
            {
                return [];
            }

            int length = q.Length();

            if (index >= length || index < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            if (count <= 0)
            {
                throw new ArgumentException("Count has to be greater than 0", "count");
            }

            if (index + count > length)
            {
                throw new ArgumentException("Will leave boundaries of q", "count");
            }

            System.Collections.Generic.List<T> listToReturn = [];

            Queue<T> temp = new();

            while (!q.IsEmpty())
            {
                temp.Insert(q.Head());
            }

            

            for (int i = 0; i < index; i++)
            {
                temp.Remove();
            }

            for (int i = 0; i < count; i++)
            {
                listToReturn.Add(temp.Head());
                q.Insert(temp.Remove());
            }

            return listToReturn;
        }

        //----------------UNNECCESARY METHODS----------------
        /// <summary>
        /// Searches for elements that match the conditions defined by the specified predicate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q">Your queue</param>
        /// <param name="match">The given predicate</param>
        /// <exception cref="ArgumentNullException">Thrown when either q or match is null</exception>
        /// <returns>A list which contains all the elements that match the conditions defined by the predicate, otherwise []</returns>
        public static System.Collections.Generic.List<T> Find<T>(this Queue<T> q, Predicate<T> match)
        {
            ArgumentNullException.ThrowIfNull(q);

            ArgumentNullException.ThrowIfNull(match);

            Queue<T> tmp = new();
            System.Collections.Generic.List<T> items = [];

            while (!q.IsEmpty())
            {
                tmp.Insert(q.Remove());
            }

            while (!tmp.IsEmpty())
            {
                if (match(tmp.Head()))
                {
                    items.Add(tmp.Head());
                }

                q.Insert(tmp.Remove());
            }
            return items;
        }

        /// <summary>
        /// Converts all elements of the input queue to the specified result type using the provided converter function.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the input queue.</typeparam>
        /// <typeparam name="TResult">The type of elements in the result queue.</typeparam>
        /// <param name="q">The input queue to be converted.</param>
        /// <param name="converter">The converter function that defines the conversion logic.</param>
        /// <param name="resultQueue">When this method returns, contains the queue that contains the converted elements.</param>
        /// <returns>The queue containing the converted elements.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="q"/> or <paramref name="converter"/> is null.</exception>
        public static Queue<TResult> ConvertAll<TSource, TResult> (this Queue<TSource> q, Converter<TSource,TResult> converter, out Queue<TResult> resultQueue)
        {
            ArgumentNullException.ThrowIfNull(q);
            ArgumentNullException.ThrowIfNull(converter);


            resultQueue = new Queue<TResult>();
            Queue<TSource> tmp = new();

            while (!q.IsEmpty())
            {
                tmp.Insert(q.Remove());
            }

            while (!tmp.IsEmpty())
            {
                TResult result = converter(tmp.Head());
                resultQueue.Insert(result);

                q.Insert(tmp.Remove());
            }

            return resultQueue;
        }
        //---------------------------------------------------

        /// <summary>
        /// Swaps the elements with the given indices within the queue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q">Your queue</param>
        /// <param name="index1">The first index</param>
        /// <param name="index2">The second index</param>
        /// <exception cref="ArgumentNullException">Thrown when q is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when either index1 or index2 are out of bounds</exception>
        public static void Swap<T> (this Queue<T> q, int index1, int index2)
        {
            ArgumentNullException.ThrowIfNull (q);
            if (q.IsEmpty()) {
                return;
            }

            int length = q.Length();

            if (index1 < 0 || index1 >= length)
            {
                throw new ArgumentOutOfRangeException("index1");
            }

            if (index2 < 0 || index2 >= length)
            {
                throw new ArgumentOutOfRangeException("index2");
            }

            System.Collections.Generic.List<T> tmpList = DequeueToList(q); // Use list for easier manipulation

            (tmpList[index2], tmpList[index1]) = (tmpList[index1], tmpList[index2]); // Swap values

            foreach (var item in tmpList)
            {
                q.Insert(item);
            }
        }

        /// <summary>
        /// Replaces all instances of a given value with another given value within the queue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q">Your queue</param>
        /// <param name="valToFind">The value you want to find</param>
        /// <param name="valToReplaceWith">The value you want to replace with the value you want to find</param>
        /// <exception cref="ArgumentNullException">Thrown when q is null</exception>
        public static void Replace<T> (this Queue<T> q, T valToFind, T valToReplaceWith)
        {
            ArgumentNullException.ThrowIfNull (q);
            if (!q.Contains(valToFind))
            {
                return; // Value isn't even in the queue, nothing to replace
            }

            System.Collections.Generic.List<T> tmpList = DequeueToList(q); // Convert to list for easier manipulation

            for (int i = 0;  i < tmpList.Count; i++) // Iterate through the list
            {
                var item = tmpList[i];
                if (System.Collections.Generic.EqualityComparer<T>.Default.Equals(item, valToFind)) // Compare current item with the value we want to find
                {
                    tmpList[i] = valToReplaceWith; // Replace the values
                }
                q.Insert(item);
            }
        }

        /// <summary>
        /// Replaces all items within the queue that match a custom logic with a different item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q">Your queue</param>
        /// <param name="valToReplaceWith">The value you want to replace your items with</param>
        /// <param name="match">The predicate by which to judge the items</param>
        /// <exception cref="ArgumentNullException">Thrown when either q or match is null</exception>
        public static void Replace<T>(this Queue<T> q, T valToReplaceWith, Predicate<T> match)
        {
            ArgumentNullException.ThrowIfNull(q);

            ArgumentNullException.ThrowIfNull(match);

            if (q.IsEmpty())
            {
                return;
            }

            System.Collections.Generic.List<T> tmpList = DequeueToList(q); // Convert to list for easier manipulation

            for (int i = 0; i < tmpList.Count; i++) // Iterate through the list
            {
                var item = tmpList[i];
                if (match(item)) // Find item that matches the Predicate
                {
                    tmpList[i] = valToReplaceWith; // Replace the values
                }
                q.Insert(item);
            }
         }

        /// <summary>
        /// Gets the similar elements between both queues
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q1">First queue</param>
        /// <param name="q2">Second queue</param>
        /// <param name="lResult">The list you want to insert the values into</param>
        /// <exception cref="ArgumentNullException">Thrown if either q1 or q2 are null</exception>
        /// <returns>A list which contains all the elements which are in both of the given queues</returns>
        public static System.Collections.Generic.List<T> Intersect<T>(Queue<T> q1, Queue<T> q2, out System.Collections.Generic.List<T> lResult)
        {
            ArgumentNullException.ThrowIfNull (q1);
            ArgumentNullException.ThrowIfNull(q2);

            lResult = [];

            if (q1.IsEmpty() || q2.IsEmpty()) // If either of the queues are empty, there cannot be an intersection
            {
                return lResult;
            }

            System.Collections.Generic.HashSet<T> hash1 = new((System.Collections.Generic.IEnumerable<T>)q1);
            System.Collections.Generic.HashSet<T> hash2 = new((System.Collections.Generic.IEnumerable<T>)q2);

            System.Collections.Generic.IEnumerable<T> intersection = hash1.Intersect(hash2); // Use the existing Intersect method

            lResult = intersection.ToList();

            return lResult;
        }

        /// <summary>
        /// Gets all elements from each queue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q1">First queue</param>
        /// <param name="q2">Second queue</param>
        /// <param name="lResult">The outputted list you want to put the values into</param>
        /// <exception cref="ArgumentNullException">Thrown when either q1 or q2 are null</exception>
        /// <returns>A list which represents the Union between both queues</returns>
        public static System.Collections.Generic.List<T> Union<T> (Queue<T> q1, Queue<T> q2, out System.Collections.Generic.List<T> lResult)
        {
            ArgumentNullException.ThrowIfNull (q1);
            ArgumentNullException.ThrowIfNull (q2);

            lResult = [];

            if (q1.IsEmpty()) // Union is q2, return only that (as a list)
            {
                lResult = new System.Collections.Generic.List<T>((System.Collections.Generic.IEnumerable<T>)q2);

                return lResult;
            }

            if (q2.IsEmpty()) // Union is q1, return only that (as a list)
            {
                lResult = new System.Collections.Generic.List<T>((System.Collections.Generic.IEnumerable<T>)q1);

                return lResult;
            }

            lResult.AddRange((System.Collections.Generic.IEnumerable<T>)q1);
            lResult.AddRange((System.Collections.Generic.IEnumerable<T>)q2);

            return lResult;
        }

        /// <summary>
        /// Gets all elements that are in q1 but not in q2
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q1">The queue you want to 'detox'</param>
        /// <param name="q2">The 'toxin'</param>
        /// <param name="lResult">The list containing the Difference between q1 and q2 (stored)</param>
        /// <returns>A list containing the Difference between q1 and q2</returns>
        public static System.Collections.Generic.List<T> Difference<T> (this Queue<T> q1, Queue<T> q2, out System.Collections.Generic.List<T> lResult)
        {
            ArgumentNullException.ThrowIfNull(q1);
            ArgumentNullException.ThrowIfNull(q2);

            lResult = [];

            if (q1.IsEmpty()) 
            {
                return lResult;
            }

            if (q2.IsEmpty())
            {
                lResult = new System.Collections.Generic.List<T>((System.Collections.Generic.IEnumerable<T>)q1);

                return lResult;
            }

            var values = new System.Collections.Generic.HashSet<T>((System.Collections.Generic.IEnumerable<T>)q1);

            var items2 = new System.Collections.Generic.HashSet<T>((System.Collections.Generic.IEnumerable<T>)q2);

            foreach (var item in values)
            {
                if (!items2.Contains(item))
                {
                    lResult.Add(item);
                }
            }

            return lResult;
        }
    }
}   
