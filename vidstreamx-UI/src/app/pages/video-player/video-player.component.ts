import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { VideoService } from '../../services/video.service';

@Component({
  selector: 'app-video-player',
  templateUrl: './video-player.component.html',
  styleUrls: ['./video-player.component.css']
})
export class VideoPlayerComponent implements OnInit {

  video: any;
  loading = true;

  constructor(private route: ActivatedRoute, private vs: VideoService) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    this.vs.getVideoById(id!).subscribe({
      next: res => { this.video = res; this.loading = false; },
      error: () => { this.loading = false; }
    });
  }
}
