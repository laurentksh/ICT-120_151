import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { GlobalAppEventsService } from 'src/app/services/global-app-events/global-app-events.service';
import { OperationResult } from 'src/app/services/models/operation-result';
import { Publication } from '../../models/publication';
import { PublicationService } from '../../services/publication.service';

@Component({
  selector: 'app-reply',
  templateUrl: './reply.component.html',
  styleUrls: ['./reply.component.css']
})
export class ReplyComponent implements OnInit {
  @Input() Publication: Publication;
  @Output() Result = new EventEmitter<OperationResult<Publication>>();

  constructor(private publicationService: PublicationService, private appEvents: GlobalAppEventsService) { }

  ngOnInit(): void {
  }

  doReply(): void {

  }

}
