using System.Numerics;
using System.Runtime.InteropServices.JavaScript;

namespace Research.Shared.Leetcode;

public static class AddTwoNumbers
{
    public static ListNode Add(ListNode l1, ListNode l2)
    {
        BigInteger listNodeToNumber(ListNode l)
        {
            var p = BigInteger.One;
            var number = BigInteger.Zero;

            for (; l != null; l = l.next)
            {
                number += l.val * p;
                p *= 10;
            }
            
            return number;
        }

        ListNode numberToListNode(BigInteger n)
        {
            ListNode s = null;
            ListNode l = null;
            
            var r = n;
            while (r > 0)
            {
                var v = r % 10;
                r /= 10;
                
                if (l == null)
                {
                    l = new ListNode((int)v);
                    s = l;
                }
                else
                {
                    l.next = new ListNode((int)v);
                    l = l.next;
                }
            }

            return s;
        }
        
        var l1Number = listNodeToNumber(l1);
        var l2Number = listNodeToNumber(l2);
        
        var res = l1Number + l2Number;
        if (res == 0)
            return new ListNode(0);
        
        var newListNode = numberToListNode(res);
        
        return newListNode;
    }
    
    public class ListNode
    {
        public int val;
        public ListNode next;

        public ListNode(int val = 0, ListNode next = null)
        {
            this.val = val;
            this.next = next;
        }
        
        public static ListNode FromArray(params int[] values)
        {
            ListNode dummy = new ListNode();
            ListNode current = dummy;

            foreach (int v in values)
            {
                current.next = new ListNode(v);
                current = current.next;
            }

            return dummy.next;
        }
        
        public int[] ToArray()
        {
            var values = new List<int>();
            var node = this;

            while (node != null)
            {
                values.Add(node.val);
                node = node.next;
            }

            return values.ToArray();
        }
    }
}