package com.example.arsplayer

import android.Manifest
import android.content.pm.PackageManager
import android.os.Bundle
import android.view.View
import android.widget.Button
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.core.app.ActivityCompat
import androidx.core.content.ContextCompat
import androidx.fragment.app.add
import androidx.fragment.app.commit


class MainActivity : AppCompatActivity() {

    lateinit var btnAudio: Button
    lateinit var btnVideo: Button
    lateinit var frgAudio: View
    lateinit var frgVideo: View

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        checkAndRequestForPermission()

        btnAudio = findViewById(R.id.btnAudio)
        btnVideo = findViewById(R.id.btnVideo)
        frgAudio = findViewById(R.id.audio_list_fragment)
        frgVideo = findViewById(R.id.video_list_fragment)

        btnAudio.setOnClickListener{
            startAudioListFragment()
        }

        btnVideo.setOnClickListener{
            startVideoListFragment()
        }

    }

    private fun startAudioListFragment() {
        supportFragmentManager.commit {
            setReorderingAllowed(true)
            add<AudioListFragment>(R.id.audio_list_fragment)
            frgVideo.visibility = View.INVISIBLE;
            frgAudio.visibility = View.VISIBLE;
        }
    }

    private fun startVideoListFragment() {
        supportFragmentManager.commit {
            setReorderingAllowed(true)
            add<VideoListFragment>(R.id.video_list_fragment)
            frgAudio.visibility = View.INVISIBLE;
            frgVideo.visibility = View.VISIBLE;
        }
    }


    private fun checkAndRequestForPermission() {
        if (ContextCompat.checkSelfPermission(
                this@MainActivity,
                Manifest.permission.READ_EXTERNAL_STORAGE
            ) !=
            PackageManager.PERMISSION_GRANTED
        ) {
            if (ActivityCompat.shouldShowRequestPermissionRationale(
                    this@MainActivity,
                    Manifest.permission.READ_EXTERNAL_STORAGE
                )
            ) {
                Toast.makeText(
                    this@MainActivity,
                    "Please accept for required permission",
                    Toast.LENGTH_SHORT
                ).show()
            } else {
                ActivityCompat.requestPermissions(
                    this@MainActivity,
                    arrayOf(Manifest.permission.READ_EXTERNAL_STORAGE),
                    1
                )
            }
        }
    }
}