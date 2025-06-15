<template>
  <div class="signature-section">
    <div class="grid grid-cols-2 gap-8 mt-8">
      <div class="signature-box text-center">
        <div class="signature-line"></div>
        <p class="signature-label">PËRGATITUR / HARTUAR NGA</p>
        <p class="signature-sublabel">(Prepared / Compiled By)</p>
      </div>

      <div class="signature-box text-center">
        <div class="signature-line"></div>
        <p class="signature-label">PRANUAR / MIRATUAR NGA</p>
        <p class="signature-sublabel">(Accepted / Approved By)</p>
      </div>
    </div>
  </div>
</template>

<script setup>
// No props needed for this component
</script>

<style scoped>
.signature-section {
  margin-top: 40px;
  page-break-inside: avoid;
}

.signature-box {
  padding: 10px;
}

.signature-line {
  border-top: 1px solid #333;
  margin: 40px auto 10px auto;
  width: 80%;
}

.signature-label {
  font-size: 13px;
  font-weight: 600;
  color: #333;
  margin: 5px 0 2px 0;
}

.signature-sublabel {
  font-size: 11px;
  color: #666;
  font-style: italic;
  margin: 0;
}

/* Print stilleri */
@media print {
  .signature-section {
    margin-top: 30px !important;
    page-break-inside: avoid !important;
    break-inside: avoid !important;
  }
  
  .grid {
    display: grid !important;
    grid-template-columns: 1fr 1fr !important;
    gap: 4cm !important;
  }
  
  .signature-line {
    border-top: 1px solid #000 !important;
    margin: 30px auto 8px auto !important;
    width: 70% !important;
  }
  
  .signature-label {
    font-size: 10px !important;
    font-weight: 600 !important;
    color: #000 !important;
  }
  
  .signature-sublabel {
    font-size: 8px !important;
    color: #333 !important;
  }
}
</style>
