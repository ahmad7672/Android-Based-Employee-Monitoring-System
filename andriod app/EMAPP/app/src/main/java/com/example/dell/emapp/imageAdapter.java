package com.example.dell.emapp;

import android.content.Context;
import android.graphics.Bitmap;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.GridView;
import android.widget.ImageView;

import java.util.ArrayList;



public class imageAdapter extends BaseAdapter {
    private Context mContext;
    int imageTotal = 4;
    ArrayList<Bitmap> bitmap;


    public imageAdapter(Context c,ArrayList<Bitmap> bm) {
        mContext = c;
        bitmap = bm;
    }
    @Override
    public int getCount() {
        return imageTotal;
    }

    @Override
    public Bitmap getItem(int position) {
        return bitmap.get(position);
    }

    @Override
    public long getItemId(int position) {
        return 0;
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        ImageView imageView;
        if (convertView == null) {
            imageView = new ImageView(mContext);
            imageView.setLayoutParams(new GridView.LayoutParams(480, 480));
            imageView.setScaleType(ImageView.ScaleType.CENTER_CROP);
            imageView.setPadding(8, 8, 8, 8);
        } else {
            imageView = (ImageView) convertView;
        }
        imageView.setImageBitmap(bitmap.get(position));
        return imageView;
    }
}
