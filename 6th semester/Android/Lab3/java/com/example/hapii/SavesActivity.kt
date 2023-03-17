package com.example.hapii

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.ImageButton

class SavesActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_saves)
        val buttonBack = findViewById<ImageButton>(R.id.btnBack)
        buttonBack.setOnClickListener {
            finish() // way 1
        }
    }
}