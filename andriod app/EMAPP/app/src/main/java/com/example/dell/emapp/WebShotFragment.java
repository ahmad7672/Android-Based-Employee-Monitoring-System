package com.example.dell.emapp;


import android.content.Context;
import android.content.ContextWrapper;
import android.content.Intent;
import android.graphics.Bitmap;
import android.os.Bundle;
import android.os.Handler;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.ProgressBar;
import android.widget.Spinner;

import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.Statement;
import java.util.ArrayList;


/**
 * A simple {@link Fragment} subclass.
 */
public class WebShotFragment extends Fragment {



    MyAsyncTask sData;
    ControlAsyncTask sControl;
    PicAsyncTask gPic;
    Bitmap bitmap;
    String task;
    Spinner spinner;
    Button btn_get;
    Connection con = null;
    Statement stmt = null,stmt2 = null, stmt3 = null;
    ResultSet rs, rs2;
    ArrayList<String> data;
    ArrayAdapter NoCoreAdapter;
    ImageView imageView;
    ProgressBar progressBar;
    public WebShotFragment() {
        // Required empty public constructor
    }
    final Handler h = new Handler();

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View v = inflater.inflate(R.layout.fragment_web_shot, container, false);
        spinner = (Spinner) v.findViewById(R.id.spinner5);
        imageView = (ImageView) v.findViewById(R.id.imageView);
        progressBar = (ProgressBar) v.findViewById(R.id.progressBar2);

        btn_get = (Button) v.findViewById(R.id.button3);

        try {
            sData = new MyAsyncTask();
            data = sData.doInBackground();

            NoCoreAdapter = new ArrayAdapter(getActivity(),android.R.layout.simple_list_item_1,data);
            spinner.setAdapter(NoCoreAdapter);

            btn_get.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    if (spinner != null && spinner.getSelectedItem() != null){
                        String name = spinner.getSelectedItem().toString();

                        progressBar.setVisibility(View.VISIBLE);



                        try{
                            task = "7";
                            sControl = new ControlAsyncTask();
                            sControl.execute(task,spinner.getSelectedItem().toString());
                        }catch (Exception e){
                            e.printStackTrace();
                        }

                        Runnable r1 = new Runnable() {

                            @Override
                            public void run() {
                                try {
                                    progressBar.setVisibility(View.GONE);
                                    gPic = new PicAsyncTask();
                                    bitmap = gPic.doInBackground("Web",spinner.getSelectedItem().toString());
                                    imageView.setImageBitmap(bitmap);

                                    imageView.setOnClickListener(new View.OnClickListener() {
                                        @Override
                                        public void onClick(View v) {
                                            /*if(isImageFitToScreen) {
                                                isImageFitToScreen=false;
                                                imageView.setLayoutParams(new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.WRAP_CONTENT, RelativeLayout.LayoutParams.WRAP_CONTENT));
                                                imageView.setAdjustViewBounds(true);
                                            }else{
                                                isImageFitToScreen=true;
                                                imageView.setLayoutParams(new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MATCH_PARENT, RelativeLayout.LayoutParams.WRAP_CONTENT));
                                                imageView.setScaleType(ImageView.ScaleType.FIT_XY);
                                            }*/

                                            String path = saveToInternalStorage(bitmap);

                                            /*ByteArrayOutputStream stream = new ByteArrayOutputStream();
                                            bitmap.compress(Bitmap.CompressFormat.PNG, 100, stream);
                                            byte[] byteArray = stream.toByteArray();*/
                                            Intent intent = new Intent(v.getContext(),LightBoxActivity.class);
                                            intent.putExtra("image",path);
                                            startActivity(intent);
                                        }
                                    });
                                } catch (Exception e) {
                                    e.printStackTrace();
                                }

                            }
                            };





                        h.postDelayed(r1,9000);

                    }



                }
            });
        }
        catch (Exception e){
            e.printStackTrace();
        }












        return v;
    }
    private String saveToInternalStorage(Bitmap bitmapImage){
        ContextWrapper cw = new ContextWrapper(getContext());
        // path to /data/data/yourapp/app_data/imageDir
        File directory = cw.getDir("imageDir", Context.MODE_PRIVATE);
        // Create imageDir
        File mypath=new File(directory,"picture.jpg");

        FileOutputStream fos = null;
        try {
            fos = new FileOutputStream(mypath);
            // Use the compress method on the BitMap object to write image to the OutputStream
            bitmapImage.compress(Bitmap.CompressFormat.PNG, 100, fos);
        } catch (Exception e) {
            e.printStackTrace();
        } finally {
            try {
                fos.close();
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        return directory.getAbsolutePath();

    }
}
