package com.example.dell.emapp;

import android.annotation.SuppressLint;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.os.AsyncTask;
import android.os.StrictMode;
import android.util.Log;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;



public class multiplePicAsynTask extends AsyncTask<String,String,ArrayList<Bitmap>> {
    String z = "";
    Boolean isSuccess = false;
    Connection con;
    ArrayList data = null;
    ArrayList<Bitmap> bitmap = new ArrayList<Bitmap>();
    Bitmap bm,bm2;


    @Override
    protected void onPreExecute()
    {

    }

    @Override
    protected void onPostExecute(ArrayList<Bitmap> params)
    {

    }
    @Override
    protected ArrayList<Bitmap> doInBackground(String... params)
    {

        try
        {
            con = connectionclass();        // Connect to database
            if (con == null)
            {
                data.add("None");
            }
            else
            {
                // Change below query according to your own database.
                String query = "Select Top 4 " + params[0] + " from pData where clientID='" + params[1] + "' order by id desc";
                Statement stmt = con.createStatement();
                ResultSet rs = stmt.executeQuery(query);
                byte[] img = null;
                while (rs.next()) {
                    img = rs.getBytes(params[0]);
                    bm = BitmapFactory.decodeByteArray(img, 0, img.length);
                    bitmap.add(bm);
                }

                //Toast.makeText(getActivity().getApplicationContext(),img.toString(),Toast.LENGTH_LONG).show();

                return bitmap;
            }
        }
        catch (Exception ex)
        {
            isSuccess = false;
            z = ex.getMessage();
        }
        return bitmap;
    }

    @SuppressLint("NewApi")
    public Connection connectionclass()
    {
        StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();
        StrictMode.setThreadPolicy(policy);
        Connection connection = null;
        String ConnectionURL = null;
        try
        {
            Class.forName("net.sourceforge.jtds.jdbc.Driver");
            ConnectionURL = "jdbc:jtds:sqlserver://MyPDBPCC.mssql.somee.com;user=cma93_SQLLogin_1;password=jztlqk3kqs";
            connection = DriverManager.getConnection(ConnectionURL);
        }
        catch (SQLException se)
        {
            Log.e("error here 1 : ", se.getMessage());
        }
        catch (ClassNotFoundException e)
        {
            Log.e("error here 2 : ", e.getMessage());
        }
        catch (Exception e)
        {
            Log.e("error here 3 : ", e.getMessage());
        }
        return connection;
    }
}
