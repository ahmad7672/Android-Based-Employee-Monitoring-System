package com.example.dell.emapp;

import android.annotation.SuppressLint;
import android.os.AsyncTask;
import android.os.StrictMode;
import android.util.Log;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;



public class MyAsyncTask extends AsyncTask<Void,Void,ArrayList<String>> {
    String z = "";
    Boolean isSuccess = false;
    Connection con;
    ArrayList data = null;



    @Override
    protected void onPreExecute()
    {

    }

    @Override
    protected void onPostExecute(ArrayList<String> params)
    {

    }
    @Override
    protected ArrayList<String> doInBackground(Void... params)
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
                    String query = "select name from table1";
                    Statement stmt = con.createStatement();
                    ResultSet rs = stmt.executeQuery(query);
                    data = new ArrayList<String>();

                    while(rs.next())
                    {
                        String id = rs.getString("name");
                        data.add(id);
                    }
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                z = ex.getMessage();
            }
        return data;
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
