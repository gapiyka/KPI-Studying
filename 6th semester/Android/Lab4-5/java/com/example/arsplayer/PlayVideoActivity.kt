package com.example.arsplayer

import android.content.Context
import android.content.Intent
import android.hardware.Sensor
import android.hardware.SensorEvent
import android.hardware.SensorEventListener
import android.hardware.SensorManager
import android.net.Uri
import android.os.Bundle
import android.os.Handler
import android.provider.Settings
import android.util.Log
import android.view.MotionEvent
import android.view.WindowManager
import android.widget.FrameLayout
import android.widget.MediaController
import android.widget.Toast
import android.widget.VideoView
import androidx.appcompat.app.AppCompatActivity


class PlayVideoActivity : AppCompatActivity(), SensorEventListener {

    private lateinit var video: VideoView
    private var brightness = 0
    private var width = 0
    private var height = 0
    private var deltaPointerDown = 0.0// відстань між пальцями
    private var deltaPointerMove = 0.0// переміщення пальця
    private var pinch = false
    private lateinit var manager: SensorManager
    private lateinit var sensors: List<Sensor>

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_play_video)

        VideoInit()
        video.start()
        VideoListener()
        checkSystemWritePermission()
        SensorInit()
    }

    override fun onAccuracyChanged(sensor: Sensor?, accuracy: Int) {
        return
    }

    override fun onSensorChanged(event: SensorEvent?) {
        if (event?.sensor?.type == Sensor.TYPE_LIGHT) {
            val light = event.values[0]
            brightness = (getBright(light) * 255).toInt()
            Settings.System.putInt(contentResolver, Settings.System.SCREEN_BRIGHTNESS, brightness)
            val layoutpars: WindowManager.LayoutParams = window.attributes
            layoutpars.screenBrightness = light
            window.attributes = layoutpars
            Log.d("BRIGHTNESS", brightness.toString())
            Log.d("light", light.toString())
        }
    }

    private fun getBright(brightness: Float): Float {
        return when (brightness.toInt()) {
            0 -> 1.0f
            in 1..10 -> 0.8f
            in 11..50 -> 0.6f
            in 51..5000 -> 0.4f
            in 5001..25000 -> 0.2f
            else -> 0.0f
        }
    }

    private fun checkSystemWritePermission() {
        var retVal = Settings.System.canWrite(this)
        if (!retVal) {
            Toast.makeText(this, "Write not allowed :-(", Toast.LENGTH_LONG).show()
            val intent = Intent(Settings.ACTION_MANAGE_WRITE_SETTINGS)
            intent.data = Uri.parse("package:" + applicationContext!!.packageName)
            intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK)
            startActivity(intent)
        }
    }

    private fun SensorInit(){
        Settings.System.putInt(getContentResolver(),
            Settings.System.SCREEN_BRIGHTNESS_MODE,
            Settings.System.SCREEN_BRIGHTNESS_MODE_MANUAL);

        brightness = Settings.System.getInt(getContentResolver(),
            Settings.System.SCREEN_BRIGHTNESS);

        manager = getSystemService(Context.SENSOR_SERVICE) as SensorManager
        sensors = manager.getSensorList(Sensor.TYPE_LIGHT)
        if(sensors.isNotEmpty()) {
            manager.registerListener(this, sensors[0], SensorManager.SENSOR_DELAY_NORMAL)
        }
        else {
            Toast.makeText(
                applicationContext,
                "The sensor is missing",
                Toast.LENGTH_LONG
            ).show()
        }
    }

    private fun VideoListener() {
        video.setOnTouchListener{ v, event ->
            val deltaX : Double
            val deltaY : Double
            when(event.action) {
                MotionEvent.ACTION_DOWN -> {
                    pinch = false
                }
                MotionEvent.ACTION_POINTER_DOWN -> {
                    pinch = true
                    deltaX = event.getX(0) - event.getX(1).toDouble()
                    deltaY = event.getY(0) - event.getY(1).toDouble()
                    deltaPointerDown = Math.sqrt(deltaX * deltaX + deltaY * deltaY)
                }
               MotionEvent.ACTION_MOVE -> {
                   if (pinch) {
                       deltaX = event.getX(0) - event.getX(1).toDouble();
                       deltaY = event.getY(0) - event.getY(1).toDouble()
                       deltaPointerMove = Math.sqrt(
                           deltaX * deltaX + deltaY * deltaY)
                       setScaledSize()
                   }
               }
                MotionEvent.ACTION_UP -> {
                    pinch = false
                }
                MotionEvent.ACTION_POINTER_UP -> {
                    pinch = false
                }
            }
            return@setOnTouchListener true;
        }
    }

    private fun setScaledSize() {
        val curScale = deltaPointerMove / deltaPointerDown
        val newHeight = (height * curScale).toInt()
        val newWidth = (width * curScale).toInt()

        video.layoutParams = FrameLayout.LayoutParams(newWidth, newHeight) //fix here (crop size)
    }

    private fun VideoInit(){
        val videoFile = intent.getParcelableExtra<VideoFile>("videoFile")
        video = findViewById<VideoView>(R.id.videoView)
        val mediaController = MediaController(this)
        mediaController.setAnchorView(video)
        video.setMediaController(mediaController)
        video.setVideoURI(videoFile!!.uri)
        //https://images.all-free-download.com/footage_preview/mp4/cute_cat_relax_on_outdoor_ground_6892533.mp4
        //https://github.com/sannies/mp4parser/blob/master/examples/src/main/resources/1365070268951.mp4
        video.requestFocus()
        deltaPointerMove = 1.0
        deltaPointerDown = 1.0
        Handler().postDelayed({
            width = video.layoutParams.width
            height = video.layoutParams.height
            setScaledSize()
        }, 100)
    }
}