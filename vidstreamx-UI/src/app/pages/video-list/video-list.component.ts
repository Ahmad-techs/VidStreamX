import { Component } from '@angular/core';
import { VideoService } from 'src/app/services/video.service';

@Component({
  selector: 'app-video-list',
  templateUrl: './video-list.component.html',
  styleUrls: ['./video-list.component.css']
})
export class VideoListComponent {
    videos: any[] = [];
  isLoading = true;

  constructor(private vs: VideoService) {}

  ngOnInit(): void {
    this.vs.getVideos().subscribe({
      next: (res: any) => {
        this.videos = res;
        this.isLoading = false;
      },
      error: (err: any) => {
        console.error("Failed to load videos", err);
        this.isLoading = false;
      }
    });
  }

}
