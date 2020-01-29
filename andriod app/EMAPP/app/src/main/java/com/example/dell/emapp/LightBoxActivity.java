package com.example.dell.emapp;

import android.content.res.Configuration;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Matrix;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;


public class LightBoxActivity extends AppCompatActivity {


    GestureImageView lightbox;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_light_box);


        lightbox = (GestureImageView) findViewById(R.id.image);


        Bundle extras = getIntent().getExtras();
        String p = extras.getString("image");
        loadImageFromStorage(p);





    }

    public Bitmap getBitmapFromSource(Object sourceValue) {
        return null;
    }

    /**
     * Determine whether the System UI should be dimmed whilst this Activity is
     * shown. Override the default value in a subclass should you wish to change it
     * @return
     */
    public boolean shouldDimUi() {
        return false;
    }

    private void loadImageFromStorage(String path)
    {
        Bitmap b;
        try {
            File f=new File(path,"picture.jpg");

            if(getResources().getConfiguration().orientation == Configuration.ORIENTATION_LANDSCAPE){
                //Do some stuff
                b = BitmapFactory.decodeStream(new FileInputStream(f));
            }
            else {
                b = RotateBitmap(BitmapFactory.decodeStream(new FileInputStream(f)),90);
            }
            lightbox.setImageBitmap(b);
            f.deleteOnExit();
        }
        catch (FileNotFoundException e)
        {
            e.printStackTrace();
        }

    }
    public static Bitmap RotateBitmap(Bitmap source, float angle)
    {
        Matrix matrix = new Matrix();
        matrix.postRotate(angle);
        return Bitmap.createBitmap(source, 0, 0, source.getWidth(), source.getHeight(), matrix, true);
    }
}
