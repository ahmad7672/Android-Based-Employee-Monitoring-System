package com.example.dell.emapp;


import android.annotation.SuppressLint;
import android.os.StrictMode;
import android.util.Log;
import java.sql.SQLException;
import java.sql.Connection;
import java.sql.DriverManager;


public class ConnectionClass {
    String ip = "MyPDBPCC.mssql.somee.com";
    String classs = "net.sourceforge.jtds.jdbc.Driver";
    String db = "MyPDBPCC.mssql.somee.com";
    String un = "cma93_SQLLogin_1";
    String password = "jztlqk3kqs";

    @SuppressLint("NewApi")
    public Connection CONN() {
        StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder()
                .permitAll().build();
        StrictMode.setThreadPolicy(policy);
        Connection conn = null;
        String ConnURL = null;
        try {

            Class.forName(classs);
            conn = DriverManager.getConnection("jdbc:jtds:sqlserver://MyPDBPCC.mssql.somee.com;user=" + un + ";password=" + password);
        } catch (SQLException se) {
            Log.e("ERRO", se.getMessage());
        } catch (ClassNotFoundException e) {
            Log.e("ERRO", e.getMessage());
        } catch (Exception e) {
            Log.e("ERRO", e.getMessage());
        }
        return conn;
    }
}
