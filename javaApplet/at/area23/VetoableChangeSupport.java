/*
 * @(#)VetoableChangeSupport.java	1.0 5/6/97
 *
 * Copyright (c) 1997 Symantec, Inc. All Rights Reserved.
 *
 */

package at.area23;

import at.area23.*;

import java.io.Serializable;
import java.io.ObjectOutputStream;
import java.io.ObjectInputStream;
import java.io.IOException;
import java.beans.PropertyVetoException;
import java.beans.PropertyChangeListener;
import java.beans.VetoableChangeListener;
import java.beans.PropertyChangeEvent;



//	05/06/97	LAB	Created

/**
 * This is a utility class that can be used by beans that support constrained
 * properties.  Your can either inherit from this class or you can use
 * an instance of this class as a member field of your bean and delegate
 * various work to it.
 * <p>
 * This extension of the java.beans.VetoableChangeSupport class adds
 * functionality to handle individual property changes.
 *
 * @author Symantec
 */
public class VetoableChangeSupport extends java.beans.VetoableChangeSupport implements java.io.Serializable
{

    /**
     * Constructs a VetoableChangeSupport object.
     * @param sourceBean the bean to be given as the source for any events
     */

    public VetoableChangeSupport(Object sourceBean)
    {
		super(sourceBean);
		source = sourceBean;
    }

    /**
     * Adds a VetoableListener to the listener list.
     *
     * @param propertyName	the name of the property to add a listener for
     * @param listener  the VetoableChangeListener to be added
     * @see #removeVetoableChangeListener
     */
    public synchronized void addVetoableChangeListener(String propertyName, VetoableChangeListener listener)
    {
    	java.util.Vector listenerList;

    	if(listenerTable == null)
    	{
    		listenerTable = new java.util.Hashtable();
    	}

    	if(listenerTable.containsKey(propertyName))
    	{
    		listenerList = (java.util.Vector)listenerTable.get(propertyName);
    	}
    	else
    	{
    		listenerList = new java.util.Vector();
    	}

    	listenerList.addElement(listener);
    	listenerTable.put(propertyName, listenerList);
    }

    /**
     * Removes a VetoableChangeListener from the listener list.
     *
     * @param propertyName	the name of the property to remove a listener for.
     * @param listener	the VetoableChangeListener to be removed
     * @see #addVetoableChangeListener
     */
    public synchronized void removeVetoableChangeListener(String propertyName, VetoableChangeListener listener)
    {
    	java.util.Vector listenerList;

		if (listenerTable == null || !listenerTable.containsKey(propertyName))
		{
	    	return;
		}
		listenerList = (java.util.Vector)listenerTable.get(propertyName);
		listenerList.removeElement(listener);
    }

    /**
     * Reports a vetoable property update to any registered listeners.
     * If anyone vetos the change, then a new event is fired reverting everyone to
     * the old value, and then the PropertyVetoException is rethrown.
     * <p>
     * No event is fired if old and new are equal and non-null.
     *
     * @param propertyName  the programmatic name of the property
     *		that was changed
     * @param oldValue  the old value of the property
     * @param newValue  the new value of the property
     *
     * @exception PropertyVetoException
     * if the specified property value is unacceptable
     */
    public void fireVetoableChange(String propertyName, Object oldValue, Object newValue) throws PropertyVetoException
    {
		if (oldValue != null && oldValue.equals(newValue))
		{
	    	return;
	    }

		super.fireVetoableChange(propertyName, oldValue, newValue);

		java.util.Hashtable templistenerTable = null;

		synchronized (this)
		{
			if(listenerTable == null || !listenerTable.containsKey(propertyName))
			{
				return;
			}
		  	templistenerTable = (java.util.Hashtable) listenerTable.clone();
		}

		java.util.Vector listenerList;

		listenerList = (java.util.Vector)templistenerTable.get(propertyName);

	    PropertyChangeEvent evt = new PropertyChangeEvent(source, propertyName, oldValue, newValue);

		try
		{
		    for (int i = 0; i < listenerList.size(); i++)
		    {
		        VetoableChangeListener target = (VetoableChangeListener)listenerList.elementAt(i);
		        target.vetoableChange(evt);
		    }
		}
		catch (PropertyVetoException veto)
		{
		    // Create an event to revert everyone to the old value.
	       	evt = new PropertyChangeEvent(source, propertyName, newValue, oldValue);
		    for (int i = 0; i < listenerList.size(); i++)
		    {
				try
				{
		            VetoableChangeListener target = (VetoableChangeListener)listenerList.elementAt(i);
		            target.vetoableChange(evt);
				}
				catch (PropertyVetoException ex)
				{
				     // We just ignore exceptions that occur during reversions.
				}
			}
			// And now rethrow the PropertyVetoException.
			throw veto;
		}
	}

	/* !!! LAB !!!	05/06/97
	If we want to support non-serializable listeners we will have to
	implement the folowing functions and serialize out the listenerTable
	HashTable on our own.

    private void writeObject(ObjectOutputStream s) throws IOException
    private void readObject(ObjectInputStream s) throws ClassNotFoundException, IOException
    */

    /**
     * The listener table.
     * @see #addVetoableChangeListener
     * @see #removeVetoableChangeListener
     */
    protected java.util.Hashtable listenerTable;
    private Object source;
    private int vetoableChangeSupportSerializedDataVersion = 1;
}
