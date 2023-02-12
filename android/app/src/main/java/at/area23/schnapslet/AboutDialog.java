/*
 *
 * @author           Heinrich Elsigan root@darkstar.work
 * @version          V 1.6.9
 * @since            API 26 Android Oreo 8.1
 *
 */
/*
   Copyright (C) 2000 - 2023 Heinrich Elsigan root@darkstar.work

   Schnapslet java applet is free software; you can redistribute it and/or
   modify it under the terms of the GNU Library General Public License as
   published by the Free Software Foundation; either version 2 of the
   License, or (at your option) any later version.
   See the GNU Library General Public License for more details.

*/
package at.area23.schnapslet;

import android.app.Dialog;
import android.content.Context;
import android.content.DialogInterface;
import android.net.Uri;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.MediaController;
import android.widget.TextView;
import android.widget.VideoView;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.appcompat.app.AlertDialog;
import androidx.fragment.app.DialogFragment;
import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentManager;

import org.jetbrains.annotations.NotNull;

import at.area23.schnapslet.R;

/**
 * A simple {@link Fragment} subclass.
 * Use the {@link AboutDialog#newInstance} factory method to
 * create an instance of this fragment.
 */
public class AboutDialog extends DialogFragment {

    /* The activity that creates an instance of this dialog fragment must
     * implement this interface in order to receive event callbacks.
     * Each method passes the DialogFragment in case the host needs to query it. */
    public interface NoticeDialogListener {
        public void onDialogPositiveClick(DialogFragment dialog);
        public void onDialogNegativeClick(DialogFragment dialog);
    }

    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "Index";
    private static final String ARG_PARAM2 = "Points";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    private int index = 9;
    public int points = 0;

    Context context;
    TextView helpTextView, builtWithTextView;
    // Use this instance of the interface to deliver action events
    NoticeDialogListener listener;


    /**
     * AboutDialog 
     * parameterless default constructor
     */
    public AboutDialog() {
        // Required empty public constructor
    }

    /**
     * newInstance
	 * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param idx index Parameter1
     * @param ptx points Parameter2
     * @return A new instance of fragment About DialogFragment.
     */
    public static AboutDialog newInstance(int idx, int ptx) {
        AboutDialog fragment = new AboutDialog();
        Bundle args = new Bundle();

        args.putInt(ARG_PARAM1, idx);
        args.putInt(ARG_PARAM2, ptx);
        fragment.setArguments(args);
        fragment.index = idx;
        fragment.points = ptx;
        return fragment;
    }

    /**
     * onAttach
	 * Override the Fragment.onAttach() method to instantiate the NoticeDialogListener
     *
     * @param c Context
     */
    @Override
    public void onAttach(Context c) {
        super.onAttach(c);
        context = c;

        // Verify that the host activity implements the callback interface
        try {
            // Instantiate the NoticeDialogListener so we can send events to the host
            listener = (NoticeDialogListener) context;
        } catch (ClassCastException e) {
            // The activity doesn't implement the interface, throw exception
            throw new ClassCastException("You must implement NoticeDialogListener" + e.getMessage());
        }
    }


    /**
     * onCreateDialog
     *
     * @param savedInstanceState Bundle
     * @return @NotNull Dialog
     */
    @Override
    public @NotNull Dialog onCreateDialog(Bundle savedInstanceState) {
        AlertDialog.Builder builder = new AlertDialog.Builder(getActivity());
        // Get the layout inflater
        LayoutInflater inflater = requireActivity().getLayoutInflater();
        final DialogFragment dialogFragment = this;

        if (savedInstanceState != null) {
            index = savedInstanceState.getInt(getString(R.string.param1_index), 0);
            points = savedInstanceState.getInt(getString(R.string.param2_points), 0);
        }

        // Inflate and set the layout for the dialog
        // Pass null as the parent view because its going in the dialog layout
        builder.setView(inflater.inflate(R.layout.dialog_fragment_about, null))
                // Add action buttons
                .setPositiveButton(R.string.learn_more, new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int id) {
                        listener.onDialogPositiveClick(dialogFragment);
                        dialogFragment.getDialog().cancel();
                    }
                })
                .setNegativeButton(R.string.back_button, new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int id) {
                        listener.onDialogNegativeClick(dialogFragment);
                        dialogFragment.getDialog().cancel();
                    }
                });
        return builder.create();
    }

    /**
     * onCreateView 
	 * Override onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
     *
     * @param inflater LayoutInflater
     * @param container ViewGroup
     * @param savedInstanceState Bundle
     * @return created View 
     */
    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        return inflater.inflate(R.layout.dialog_fragment_about, container, false);
    }


    /**
     * show 
     * Override show(@NonNull FragmentManager manager, @Nullable String tag)
     *
     * @param manager @NonNull FragmentManager
     * @param tag @Nullable String
     */
    @Override
    public void show(@NonNull FragmentManager manager, @Nullable String tag) {
        super.show(manager, tag);
    }


    /**
     * onStart 
     * Override onStart()
     */
    @Override
    public void onStart() {
        super.onStart();
        View v = null;
        try {
            v = getView();
            helpTextView = (TextView) v.findViewById(R.id.helpTextView);
			builtWithTextView = (TextView) v.findViewById(R.id.builtWithTextView);
            helpTextView.setVisibility(View.VISIBLE);
            builtWithTextView.setVisibility(View.VISIBLE);
        } catch (Exception exi) {
            exi.printStackTrace();
        }
    }

}