package com.example.hapii

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity

class MainActivity : AppCompatActivity(), SelectFragment.OnSelectedListener {
    private lateinit var db: Database

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)
        db = Database(this)
        db.onUpgrade(db.writableDatabase, 1, 2)
        val buttonOpen = findViewById<Button>(R.id.btnOpen)
        buttonOpen.setOnClickListener {
            val data = db.getArrayListOfText()
            if(data.isEmpty()){
                Toast.makeText(applicationContext, getString(R.string.empty_db),
                    Toast.LENGTH_LONG,).show()
            }
            else {
                val intent = Intent(this, SavesActivity::class.java)
                intent.putExtra("db", data)
                startActivity(intent)
            }
        }
    }

    override fun onSelected(text: String, font: Int) {
        val fragment = supportFragmentManager
            .findFragmentById(R.id.resultFragment) as ResultFragment
        fragment.setTextResult(text, font)
        if (text.isNotEmpty()) {
            db.addData(text, font)
            Toast.makeText(
                this, getString(R.string.db_input),
                Toast.LENGTH_LONG
            ).show()
        }
    }
}