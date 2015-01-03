using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace PawJershauge.IMDBFlatFiles
{
    public abstract class DataReaderList<T> : DataReader, ICollection<T>, ICollection, IEnumerable<T>, IEnumerator, IList<T>, IList where T : class
    {
        #region private fields

        private List<T> innerlist = null; // collection container for the T object
        private ISynchronizeInvoke synchronizingObject; //Contains the object to Sync up against. 
        private int iecur = -1; //Cursor for use in IEnumerator implementation
        private object root = new object(); //root object to lock when doing threadsafe calls

        #endregion

        #region Events

        /// <summary>
        /// Occurs when an T is added to the collection.
        /// </summary>
        public event EventHandler<EventArgs> ItemAdded;
        /// <summary>
        /// Occurs when an array of T is added to the collection.
        /// </summary>
        public event EventHandler<EventArgs> ItemRangeAdded;
        /// <summary>
        /// Occurs when an T is removed from the collection.
        /// </summary>
        public event EventHandler<EventArgs> ItemRemoved;
        /// <summary>
        /// Occurs when an T is inserted to the collection.
        /// </summary>
        public event EventHandler<EventArgs> ItemInserted;
        /// <summary>
        /// Occurs when the collection is cleared.
        /// </summary>
        public event EventHandler<EventArgs> ListCleared;

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the object used to marshal event-handler calls.
        /// </summary>
        [Browsable(false), DefaultValue((string)null)]
        public ISynchronizeInvoke SynchronizingObject //Property to get or set the object to Sync up against.
        {
            get
            {
                lock (root)
                { return this.synchronizingObject; }
            }
            set
            {
                lock (root)
                { this.synchronizingObject = value; }
            }
        }

        #endregion

        #region ctor

        /// <summary>
        /// Initializes a new instance of the DataReaderList<T> class with default values.
        /// </summary>
        public DataReaderList()
        {
            innerlist = new List<T>();
        }

        /// <summary>
        /// Initializes a new instance of the DataReaderList<T> class that is empty and has The specified initial capacity.
        /// </summary>
        /// <param name="capacity">The number of T that the new DataReaderList<T> can initially store.</param>
        public DataReaderList(int capacity)
        {
            innerlist = new List<T>(capacity);
        }

        /// <summary>
        /// Initializes a new instance of the DataReaderList<T> class that has The specified initial capacit and contains elements added from the specified array.
        /// </summary>
        /// <param name="capacity">The number of T that the new DataReaderList<T> can initially store.</param>
        /// <param name="array">The array whose T are added to the new DataReaderList<T>.</param>
        public DataReaderList(int capacity, params T[] array)
            : this(capacity)
        {
            if (capacity < array.Length)
                throw new ArgumentOutOfRangeException("array", string.Format("Length of array excides the capacity.\ncapacity: {0}\narray length: {1}", capacity, array.Length));
            innerlist.AddRange(array);
        }

        /// <summary>
        /// Initializes a new instance of the DataReaderList<T> class that contains T added from the specified array and has sufficient capacity to accommodate the number of T copied.
        /// </summary>
        /// <param name="capacity">The number of T that the new DataReaderList<T> can initially store.</param>
        /// <param name="array">The array whose T are added to the new DataReaderList<T>.</param>
        public DataReaderList(params T[] array)
            : this()
        {
            innerlist.AddRange(array);
        }

        /// <summary>
        /// Initializes a new instance of the DataReaderList<T> class that contains T copied from the specified collection and has sufficient capacity to accommodate the number of T copied.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new DataReaderList<T>.</param>
        public DataReaderList(IEnumerable<T> collection)
        {
            innerlist = new List<T>(collection);
        }

        #endregion

        #region ICollection<T> Members

        /// <summary>
        /// Adds an T to the DataReaderList<T>.
        /// </summary>
        /// <param name="item">The T to add to the DataReaderList<T>.</param>
        public void Add(T item)
        {
            lock (root)
            {
                innerlist.Add(item);
                OnItemAdded(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Removes all T from the DataReaderList<T>
        /// </summary>
        public void Clear()
        {
            lock (root)
            {
                innerlist.Clear();
                OnListCleared(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Determines whether the DataReaderList<T> contains a specific T.
        /// </summary>
        /// <param name="item">The T to locate in the DataReaderList<T>.</param>
        /// <returns>true if the T is found in the DataReaderList<T>; otherwise, false.</returns>
        public bool Contains(T item)
        {
            lock (root)
            {
                return innerlist.Contains(item);
            }
        }

        /// <summary>
        /// Copies the T's of the DataReaderList<T> to an Array of T, starting at a particular T Array index.
        /// </summary>
        /// <param name="array">The one-dimensional T Array that is the destination of the elements copied from DataReaderList<T>. The T Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (root)
            {
                if (arrayIndex < 0)
                    throw new ArgumentOutOfRangeException("arrayIndex", arrayIndex, "Index is out of range");
                innerlist.CopyTo(array, arrayIndex);
            }
        }

        /// <summary>
        /// Gets the number of T contained in the DataReaderList<T>.
        /// </summary>
        ///<returns>The number of T contained in the DataReaderList<T>.</returns>
        public int Count
        {
            get
            {
                lock (root)
                {
                    return innerlist.Count;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the DataReaderList<T> is read-only. Returns false always by implementation
        /// </summary>
        /// <returns>false</returns>
        public bool IsReadOnly
        {
            get
            {
                lock (root)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific T from the DataReaderList<T>.
        /// </summary>
        /// <param name="item">The T to remove from the DataReaderList<T>.</param>
        /// <returns>true if T was successfully removed from the DataReaderList<T>; otherwise, false. This method also returns false if item is not found in the original DataReaderList<T>.</returns>
        public bool Remove(T item)
        {
            lock (root)
            {
                bool rtnval = innerlist.Remove(item);
                OnItemRemoved(EventArgs.Empty);
                return rtnval;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A System.Collections.Generic.IEnumerator&lt;T&gt; that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            lock (root)
            {
                return innerlist.GetEnumerator();
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A System.Collections.Generic.IEnumerator&lt;T&gt; that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (root)
            {
                return (IEnumerator)innerlist;
            }
        }

        #endregion

        #region ICollection

        /// <summary>
        /// Copies the T's of the DataReaderList<T> to an System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from DataReaderList<T>. The System.Array must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in array at which copying begins.</param>
        public void CopyTo(Array array, int index)
        {
            lock (root)
            {
                if (object.ReferenceEquals(array, null))
                    throw new ArgumentNullException("array", "Null array reference");
                if (index < 0 || innerlist.Count < index)
                    throw new ArgumentOutOfRangeException("index", index, "Index is out of range");
                if (array.Rank > 1)
                    throw new ArgumentException("Array is multi-dimensional", "array");
                if ((innerlist.Count - index) > array.Length)
                    throw new ArgumentOutOfRangeException("array", "Length of the array is to small, the number of object to be copied, excides the size of the array");

                for (int i = index; i < innerlist.Count; i++)
                    array.SetValue(innerlist[i], (i - index));
            }
        }

        /// <summary>
        /// Gets a value indicating if access to the DataReaderList<T> is synchronized (thread safe). Returns true always by implementation
        /// </summary>
        /// <value>true because the access to the DataReaderList<T> is synchronized (thread safe)</value>
        public bool IsSynchronized
        {
            get
            {
                lock (root)
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the DataReaderList<T>.
        /// </summary>
        /// <returns>An object that can be used to synchronize access to the DataReaderList<T>.</returns>
        public object SyncRoot
        {
            get
            {
                lock (root)
                {
                    return root;
                }
            }
        }

        #endregion

        #region IEnumerable<T>

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A System.Collections.Generic.IEnumerator&lt;T&gt; that can be used to iterate through the collection.</returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            lock (root)
            {
                return innerlist.GetEnumerator();
            }
        }

        #endregion

        #region IEnumerator

        /// <summary>
        /// Gets the current element in the collection.
        /// </summary>
        /// <returns>The current element in the collection.</returns>
        public object Current
        {
            get
            {
                lock (root)
                {
                    if ((iecur < 0) || (iecur == innerlist.Count))
                        throw new InvalidOperationException();
                    return innerlist[iecur];
                }
            }
        }

        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
        public bool MoveNext()
        {
            lock (root)
            {
                if (iecur < innerlist.Count)
                    iecur++;
                return (!(iecur == innerlist.Count));
            }
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element in the collection.
        /// </summary>
        public void Reset()
        {
            lock (root)
            {
                iecur = -1;
            }
        }

        #endregion

        #region IList<T> Members

        /// <summary>
        /// Determines the index of a specific T in the DataReaderList<T>
        /// </summary>
        /// <param name="item">The T to locate in the DataReaderList<T></param>
        /// <returns>The index of T if found in the DataReaderList<T>; otherwise, -1.</returns>
        public int IndexOf(T item)
        {
            lock (root)
            {
                return innerlist.IndexOf(item);
            }
        }

        /// <summary>
        /// Inserts an T to the DataReaderList<T> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which T should be inserted.</param>
        /// <param name="item">The T to insert into the DataReaderList<T></param>
        public void Insert(int index, T item)
        {
            lock (root)
            {
                innerlist.Insert(index, item);
                OnItemInserted(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Removes the T from DataReaderList<T> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public void RemoveAt(int index)
        {
            lock (root)
            {
                innerlist.RemoveAt(index);
                OnItemRemoved(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the T at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the T to get or set.</param>
        /// <returns>The T at the specified index.</returns>
        public T this[int index]
        {
            get
            {
                lock (root)
                {
                    return innerlist[index];
                }
            }
            set
            {
                lock (root)
                {
                    innerlist[index] = value;
                }
            }
        }

        #endregion

        #region IList

        /// <summary>
        /// Adds an object to the DataReaderList<T> if the object is of type T.
        /// </summary>
        /// <param name="value">The object to add to the DataReaderList<T>.</param>
        /// <returns>The position into which the new element was inserted, or -1 to indicate that the item was not inserted into the collection.</returns>
        public int Add(object value)
        {
            lock (root)
            {
                if (!(value is T))
                    throw new ArgumentException("type of value is not T", "value");
                innerlist.Add(value as T);
                OnItemAdded(EventArgs.Empty);
                return innerlist.Count - 1;
            }
        }

        /// <summary>
        /// Determines whether the DataReaderList<T> contains a specific object of type T.
        /// </summary>
        /// <param name="value">The object of type T to locate in the DataReaderList<T>.</param>
        /// <returns>true if the T is found in the DataReaderList<T>; otherwise, false.</returns>
        public bool Contains(object value)
        {
            lock (root)
            {
                if (!(value is T))
                    throw new ArgumentException("type of value is not T", "value");
                return innerlist.Contains(value as T);
            }
        }

        /// <summary>
        /// Determines the index of a specific object of type T in the DataReaderList<T>
        /// </summary>
        /// <param name="value">The object of type T to locate in the DataReaderList<T></param>
        /// <returns>The index of object of type T if found in the DataReaderList<T>; otherwise, -1.</returns>
        public int IndexOf(object value)
        {
            lock (root)
            {
                if (!(value is T))
                    throw new ArgumentException("type of value is not T", "value");
                return innerlist.IndexOf(value as T);
            }
        }

        /// <summary>
        /// Inserts an object of type T to the DataReaderList<T> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which object of type T should be inserted.</param>
        /// <param name="value">The object of type T to insert into the DataReaderList<T></param>
        public void Insert(int index, object value)
        {
            lock (root)
            {
                if (!(value is T))
                    throw new ArgumentException("type of value is not T", "value");
                innerlist.Insert(index, value as T);
                OnItemInserted(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the DataReaderList<T> has a fixed size. Returns false always by implementation
        /// </summary>
        /// <returns>false</returns>
        public bool IsFixedSize
        {
            get
            {
                lock (root)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object of type T from the DataReaderList<T>.
        /// </summary>
        /// <param name="value">The object of type T to remove from the DataReaderList<T>.</param>
        public void Remove(object value)
        {
            lock (root)
            {
                if (!(value is T))
                    throw new ArgumentException("type of value is not T", "value");
                innerlist.Remove(value as T);
                OnItemRemoved(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the T at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the T to get or set.</param>
        /// <returns>The T at the specified index.</returns>
        object IList.this[int index]
        {
            get
            {
                lock (root)
                {
                    return innerlist[index];
                }
            }
            set
            {
                lock (root)
                {
                    if (!(value is T))
                        throw new ArgumentException("type of value is not T", "value");
                    innerlist[index] = value as T;
                }
            }
        }

        #endregion

        #region Virual Methods

        /// <summary>
        /// Raises the DataReaderList<T>.ItemAdded event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        public virtual void OnItemAdded(EventArgs e)
        {
            lock (root)
            {
                EventHandler<EventArgs> localhandler = ItemAdded;
                if (localhandler != null) //if the localhandler event is not null. (meaning that an subscriber to the event is present...)
                {
                    if ((this.SynchronizingObject != null) && this.SynchronizingObject.InvokeRequired) //SynchronizingObject is not null and the Synchronizing Object Requires and invokation...
                        this.SynchronizingObject.BeginInvoke(localhandler, new object[] { this, e }); //Get the Synchronizing Object to invoke the event...
                    else
                        localhandler(this, e); //Fire the event
                }
            }
        }

        /// <summary>
        /// Raises the DataReaderList<T>.ItemRangeAdded event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        public virtual void OnItemRangeAdded(EventArgs e)
        {
            lock (root)
            {
                EventHandler<EventArgs> localhandler = ItemRangeAdded;
                if (localhandler != null) //if the localhandler event is not null. (meaning that an subscriber to the event is present...)
                {
                    if ((this.SynchronizingObject != null) && this.SynchronizingObject.InvokeRequired) //SynchronizingObject is not null and the Synchronizing Object Requires and invokation...
                        this.SynchronizingObject.BeginInvoke(localhandler, new object[] { this, e }); //Get the Synchronizing Object to invoke the event...
                    else
                        localhandler(this, e); //Fire the event
                }
            }
        }

        /// <summary>
        /// Raises the DataReaderList<T>.ItemRemoved event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        public virtual void OnItemRemoved(EventArgs e)
        {
            lock (root)
            {
                EventHandler<EventArgs> localhandler = ItemRemoved;
                if (localhandler != null) //if the localhandler event is not null. (meaning that an subscriber to the event is present...)
                {
                    if ((this.SynchronizingObject != null) && this.SynchronizingObject.InvokeRequired) //SynchronizingObject is not null and the Synchronizing Object Requires and invokation...
                        this.SynchronizingObject.BeginInvoke(localhandler, new object[] { this, e }); //Get the Synchronizing Object to invoke the event...
                    else
                        localhandler(this, e); //Fire the event
                }
            }
        }

        /// <summary>
        /// Raises the DataReaderList<T>.ItemInserted event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        public virtual void OnItemInserted(EventArgs e)
        {
            lock (root)
            {
                EventHandler<EventArgs> localhandler = ItemInserted;
                if (localhandler != null) //if the localhandler event is not null. (meaning that an subscriber to the event is present...)
                {
                    if ((this.SynchronizingObject != null) && this.SynchronizingObject.InvokeRequired) //SynchronizingObject is not null and the Synchronizing Object Requires and invokation...
                        this.SynchronizingObject.BeginInvoke(localhandler, new object[] { this, e }); //Get the Synchronizing Object to invoke the event...
                    else
                        localhandler(this, e); //Fire the event
                }
            }
        }

        /// <summary>
        /// Raises the DataReaderList<T>.ListCleared event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        public virtual void OnListCleared(EventArgs e)
        {
            lock (root)
            {
                EventHandler<EventArgs> localhandler = ListCleared;
                if (localhandler != null) //if the localhandler event is not null. (meaning that an subscriber to the event is present...)
                {
                    if ((this.SynchronizingObject != null) && this.SynchronizingObject.InvokeRequired) //SynchronizingObject is not null and the Synchronizing Object Requires and invokation...
                        this.SynchronizingObject.BeginInvoke(localhandler, new object[] { this, e }); //Get the Synchronizing Object to invoke the event...
                    else
                        localhandler(this, e); //Fire the event
                }
            }
        }

        #endregion


        public abstract object[] GetObjects(T item);

        public override long GetTotalItems()
        {
            return Count;
        }

        public override long GetCurrentItems()
        {
            return iecur;
        }

        public override bool GetIsClosed()
        {
            return Count > 0 ? (Count == iecur) : false;
        }

        public override bool Read()
        {
            SetObjectArray(new object[] { GetObjects(this.Current as T) });
            OnReading(iecur, Count);
            return MoveNext();
        }
    }
}
