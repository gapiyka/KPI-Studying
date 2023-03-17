package com.example.arsplayer

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import kotlin.time.Duration.Companion.seconds

class VideoListAdapter(
    private val videoList: ArrayList<VideoFile>,
    private val onClick: (VideoFile) -> Unit) :
    RecyclerView.Adapter<VideoListAdapter.ViewHolder>() {

    class ViewHolder(view: View, val onClick: (VideoFile) -> Unit) :
        RecyclerView.ViewHolder(view) {
        private val title: TextView = view.findViewById(R.id.title)
        private var currentVideo: VideoFile? = null

        init {
            view.setOnClickListener {
                currentVideo?.let {
                    onClick(it)
                }
            }
        }

        fun bind(video: VideoFile) {
            currentVideo = video
            title.text = video.title
        }
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ViewHolder {
        val view = LayoutInflater.from(parent.context)
            .inflate(R.layout.list_item, parent, false)
        return ViewHolder(view, onClick)
    }

    override fun onBindViewHolder(holder: ViewHolder, position: Int) {
        val video = videoList[position]
        holder.bind(video)
    }

    override fun getItemCount(): Int {
        return videoList.size
    }
}