﻿
using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryTrees
{
    public class BinaryTree<T> : IEnumerable<T> where T : IComparable
    {
        public class TreeNode<T> where T : IComparable
        {
            public T Value;
            public TreeNode<T> Left;
            public TreeNode<T> Right;
            public TreeNode<T> Parent;
            public int Count;
        }

        public TreeNode<T> RootNode;

        public void Add(T key)
        {
            if (RootNode == null)
            {
                RootNode = new TreeNode<T>() { Value = key };
                RootNode.Count++;
            }

            else
                CrossTheTree(key);
        }

        public void CrossTheTree(T key)
        {
            var node = RootNode;
            RootNode.Count++;
            while (true)
            {
                var leftBranch = InitNewBranch(node.Left, node.Value.CompareTo(key) > 0, key);
                var rightBranch = InitNewBranch(node.Right, node.Value.CompareTo(key) < 0, key);
                if (!Equals(leftBranch, null))
                {
                    node.Left = leftBranch;
                    node.Left.Parent = node;
                    break;
                }
                if (!Equals(rightBranch, null))
                {
                    node.Right = rightBranch;
                    node.Right.Parent = node;
                    break;
                }
                node = node.Value.CompareTo(key) > 0 ? node.Left : node.Right;
                node.Count++;
            }
        }

        public TreeNode<T> InitNewBranch(TreeNode<T> node, bool condition, T value)
        {
            if (condition)
            {
                if (Equals(node, null))
                {
                    var next = new TreeNode<T>() { Value = value };
                    next.Count++;
                    return next;
                }
            }
            return null;
        }

        public bool Contains(T key)
        {
            var node = RootNode;
            if (RootNode == null) return false;
            while(!Equals(node, null))
            {
                if (node.Value.Equals(key)) return true;
                node = node.Value.CompareTo(key) < 0 ? node.Right : node.Left;
            }
            return false;
        }

        public T this[int i]
        {
            get
            {
                var node = RootNode;
                int prevCount = 0;
                var curIndex = 0;
                while (true)
                {
                    curIndex = GetIndex(node, prevCount);
                    if (i == curIndex)
                        return node.Value;
                    if (i < curIndex)
                        node = node.Left;
                    else
                    {
                        prevCount = curIndex+1;
                        node = node.Right;
                    }
                }
            }
        }

        public int GetIndex(TreeNode<T> node, int prevCount)
        {
            if (Equals(node.Left, null))
                return prevCount;
            else
                return node.Left.Count + prevCount;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return CrossTreeInOrder(RootNode);
        }
        public IEnumerator<T> CrossTreeInOrder(TreeNode<T> node)
        { 
            if (node == null)
                yield break;
            var nextNode = CrossTreeInOrder(node.Left);
            while (nextNode.MoveNext())
                yield return nextNode.Current;
            yield return node.Value;
            nextNode = CrossTreeInOrder(node.Right);
            while (nextNode.MoveNext())
                yield return nextNode.Current;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
