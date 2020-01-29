package com.example.dell.emapp;

import android.os.Bundle;
import android.os.Handler;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ProgressBar;
import android.widget.Spinner;

import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.Statement;
import java.util.ArrayList;


public class PCControlFragment extends Fragment {
    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER




    // My Variables
    MyAsyncTask sData;
    ControlAsyncTask sControl;
    Spinner spinner1, spinner2;
    Button btn_set;
    ConnectionClass connectionClass;
    Connection con = null;
    Statement stmt = null;
    ResultSet rs;
    ArrayList<String> data;
    ArrayAdapter NoCoreAdapter;
    ProgressBar progressBar;
    String[] State = {"Hold","Enable","ShutDown","LogOut"};

    public PCControlFragment() {
        // Required empty public constructor

    }

 /*   WebView web;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View v = inflater.inflate(R.layout.fragment_pccontrol, container, false);
        web=(WebView) v.findViewById(R.id.webcontrol);
        web.loadUrl("http://pccontrol.hostoise.com/ControlPC.aspx");
        web.setWebViewClient(new WebViewClient());
        web.getSettings().setUseWideViewPort(false);
        web.getSettings().setBuiltInZoomControls(false);

        return v;
    }*/
    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                          Bundle savedInstanceState) {
     View v=inflater.inflate(R.layout.fragment_pccontrol,container,false);
     spinner1 = (Spinner) v.findViewById(R.id.spinner);
     spinner2 = (Spinner) v.findViewById(R.id.spinner3);
        progressBar = (ProgressBar) v.findViewById(R.id.progressBar4);
    ArrayAdapter<String> list2 = new ArrayAdapter<String>(getActivity(),android.R.layout.simple_list_item_1,State);
    spinner2.setAdapter(list2);
     btn_set = (Button) v.findViewById(R.id.button);

        try {
            sData = new MyAsyncTask();
            data = sData.doInBackground();
            NoCoreAdapter = new ArrayAdapter(getActivity(),android.R.layout.simple_list_item_1,data);
            spinner1.setAdapter(NoCoreAdapter);
            btn_set.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    if (spinner1 != null && spinner1.getSelectedItem() != null && spinner2 != null && spinner2.getSelectedItem() != null){
                        String name = spinner1.getSelectedItem().toString();
                        String st = spinner2.getSelectedItem().toString();
                        String task = "1";
                        if (spinner2.getSelectedItem() == "Enable")
                        {
                            task = "4";
                        }
                        else if (spinner2.getSelectedItem() == "Hold")
                        {
                            task = "2";
                        }
                        else if (spinner2.getSelectedItem() == "LogOut")
                        {
                            task = "5";
                        }
                        else if (spinner2.getSelectedItem() == "ShutDown")
                        {
                            task = "6";
                        }

                        try{
                            progressBar.setVisibility(View.VISIBLE);
                            sControl = new ControlAsyncTask();
                            sControl.execute(task,spinner1.getSelectedItem().toString());
                            final Handler h = new Handler();

// Create and start a new Thread
                            Runnable r1 = new Runnable() {
                                @Override
                                public void run() {
                                    progressBar.setVisibility(View.INVISIBLE);
                                }
                            };

                            h.postDelayed(r1,9000);



                        }catch (Exception e){
                            e.printStackTrace();
                        }

                    }



                }
            });
        }
        catch (Exception e){
            e.printStackTrace();
        }





     return v;
 }


}

