import { Component, OnInit } from '@angular/core';
import { BasketService } from '../basket.service';
import { Product } from '../product';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.css']
})
export class BasketComponent implements OnInit {

  products: Product[]

  constructor(private basketService: BasketService) { }

  ngOnInit() {
    this.products = this.basketService.getProducts();
  }

}
