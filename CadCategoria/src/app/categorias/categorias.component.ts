import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/services/api.service';
import { Categoria } from 'src/model/categoria';

@Component({
  selector: 'app-categorias',
  templateUrl: './categorias.component.html',
  styleUrls: ['./categorias.component.scss']
})
export class CategoriasComponent implements OnInit {
  displayedColumns: string[] = [ 'nome', 'imagem','acao'];
  dataSource: Categoria[];
  isLoadingResults = true;

  constructor(private api: ApiService) { }

  ngOnInit(): void {
    this.api.getCategorias()
    .subscribe(res => {
      this.dataSource = res;
      console.log(this.dataSource);
      this.isLoadingResults = false;
    }, err => {
      console.log(err);
      this.isLoadingResults = false;
    });
  }

}
