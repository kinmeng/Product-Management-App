// src/app/product-form/product-form.component.ts
import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProductService } from '../Services/product.service';
import { Router, ActivatedRoute } from '@angular/router';
import { Product } from '../models/product.model';

@Component({
  selector: 'app-product-form',
  templateUrl: './product-form.component.html',
  styleUrls: ['./product-form.component.css']
})
export class ProductFormComponent implements OnInit {
  productForm: FormGroup;
  isEditMode = false;
  productId: number | null = null;

  constructor(
    private fb: FormBuilder,
    private productService: ProductService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.productForm = this.fb.group({
      name: ['', Validators.required],
      quantity: [0, [Validators.required, Validators.min(0)]],
      price: [0, [Validators.required, Validators.min(0)]]
    });
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.productId = +params['id'];
      if (this.productId) {
        this.isEditMode = true;
        this.loadProduct();
      }
    });
  }

  loadProduct(): void {
    if (this.productId !== null) {
      this.productService.getProduct(this.productId).subscribe(product => {
        this.productForm.patchValue(product);
      });
    }
  }

  onSubmit(): void {
    if (this.productForm.valid) {
      const product: Product = this.productForm.value;
      if (this.isEditMode && this.productId) {
        this.productService.updateProduct(this.productId, product).subscribe(() => {
          this.router.navigate(['/products']);
        });
      } else {
        this.productService.createProduct(product).subscribe(() => {
          this.router.navigate(['/products']);
        });
      }
    }
  }
}
