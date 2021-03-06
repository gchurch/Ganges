import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Product } from '../../product'
import { ProductService } from '../../services/product.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.scss']
})
export class AddProductComponent implements OnInit {

  public productForm:  FormGroup;

  public constructor(private productService: ProductService, private router: Router) { }

  // https://angular.io/guide/reactive-forms
  // https://angular.io/guide/form-validation
  // https://angular.io/api/forms/Validators
  public ngOnInit(): void {
    this.productForm = new FormGroup({
      title: new FormControl('', Validators.required),
      description: new FormControl('', Validators.required),
      seller: new FormControl('', Validators.required),
      price: new FormControl(0, [Validators.required, Validators.min(0.01)]),
      quantity: new FormControl(0, [Validators.required, Validators.min(1)])
    });
    this.productForm.reset();
  }

  public onSubmit(): void {
    var product = this.createProductObjectFromFormData();
    console.log(product);
    this.addProduct(product);
  }

  private createProductObjectFromFormData(): Product {
    var product: Product = {
      productId: 0,
      title: this.productForm.value.title,
      description: this.productForm.value.description,
      seller: this.productForm.value.seller,
      price: parseInt(this.productForm.value.price),
      quantity: parseInt(this.productForm.value.quantity),
      imageUrl: ""
    };
    return product;
  }

  private addProduct(product: Product) : void {
    this.productService.addProduct(product).subscribe(
      output => {
        console.log(output);
        console.log("The product has been added to the database.");
        var productId: number = output.productId;
        this.productForm.reset();
        this.router.navigate(['/products/' + productId]);
      }
    );
  }

}
